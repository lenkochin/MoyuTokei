using LenChon.Win32.TrayIcon;
using LenChon.Win32.TrayIcon.Events;
using MoyuTokei.Common;
using MoyuTokei.Common.Events;
using MoyuTokei.Common.Extensions;
using MoyuTokei.Core;
using MoyuTokei.Core.Interop;
using MoyuTokei.DataExchange;
using MoyuTokei.Helpers;
using MoyuTokei.Keys;
using MoyuTokei.Services.Interfaces;
using MoyuTokei.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MoyuTokei.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly ITokeiService _tokeiService;

        private double _trayScalingCenterX, _trayScalingCenterY;
        private JudgmentMode _judgmentMode;
        private TokeiMode _mode;
        private string _monitorName;
        private int _renderLock, _dialogLock;

        private CancellationTokenSource _cancellationTokenSource;
        private List<IntPtr> _selectedAppHandles;
        private DispatcherTimer _windowCheckTimer;
        private IntPtr? _mainWindowHandle;

        public double TrayScalingCenterX
        {
            get => _trayScalingCenterX;
            private set => SetProperty(ref _trayScalingCenterX, value);
        }

        public double TrayScalingCenterY
        {
            get => _trayScalingCenterY;
            private set => SetProperty(ref _trayScalingCenterY, value);
        }

        public JudgmentMode JudgmentMode
        {
            get => _judgmentMode;
            set => SetProperty(ref _judgmentMode, value);
        }

        public TokeiMode Mode
        {
            get => _mode;
            private set => SetProperty(ref _mode, value);
        }

        public string MonitorName
        {
            get => _monitorName;
            private set => SetProperty(ref _monitorName, value);
        }

        public ICommand LoadCommand { get; private set; }

        public ICommand PrepareForTrayPopupOpeningCommand { get; private set; }

        public ICommand SwitchJudgementModeCommand { get; private set; }

        public ICommand SwitchTokeiModeCommand { get; private set; }

        public ICommand StartCapturingApplicationCommand { get; private set; }

        public ICommand DragMoveCompletedCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public ICommand RenderCommand { get; private set; }

        public ICommand ExitApplicationCommand { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager,
                                   IEventAggregator eventAggregator,
                                   IDialogService dialogService,
                                   ITokeiService tokeiService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _tokeiService = tokeiService;

            SetupCommands();
            InitializeWatchTimer();
        }

        private void SetupCommands()
        {
            LoadCommand = new DelegateCommand<FrameworkElement>(ExecuteLoad);
            PrepareForTrayPopupOpeningCommand = new DelegateCommand<TrayPopupOpenEventArgs>(ExecutePrepareForTrayPopupOpening);
            SwitchJudgementModeCommand = new DelegateCommand<JudgmentMode?>(ExecuteSwitchJudgementMode);
            SwitchTokeiModeCommand = new DelegateCommand(ExecuteSwitchTokeiMode);
            StartCapturingApplicationCommand = new DelegateCommand(ExecuteStartCapturingApplication);
            DragMoveCompletedCommand = new DelegateCommand<Window>(ExecuteDragMoveCompleted);
            ResetCommand = new DelegateCommand(ExecuteReset);
            RenderCommand = new DelegateCommand<FrameworkElement>(ExecuteRender);
            ExitApplicationCommand = new DelegateCommand(ExecuteExitApplication);
        }

        private void InitializeWatchTimer()
        {
            _windowCheckTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _windowCheckTimer.Tick += WatchTimerTickedHandler;
        }

        private void UpdateTokeiMode(TokeiMode newMode)
        {
            Mode = newMode;

            if (Mode == TokeiMode.Jixue)
            {
                _tokeiService?.SwitchToJixue();
            }
            else
            {
                _tokeiService?.SwitchToMoyu();
            }
        }

        private void UpdateJudgmentMode(JudgmentMode newMode)
        {
            JudgmentMode = newMode;
            _eventAggregator.GetEvent<JudgmentModeChangedEvent>().Publish(new(JudgmentMode));
        }

        private void UpdateMonitorName(Window window)
        {
            MonitorName = WindowHelper.GetMonitorFriendlyNameFromWindow(window);
        }

        private async Task<List<(string Title, IntPtr Handle)>> RefreshApplicationList()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();

            var result = await NativeHelper.GetVisibleWindowsAsync(_cancellationTokenSource.Token);
            var mainWindowHandle = _mainWindowHandle ??= Application.Current.MainWindow.GetHandle();

            var idx = result.FindIndex(i => i.Handle == mainWindowHandle);

            if (idx != -1)
            {
                result.RemoveAt(idx);
            }

            return result;
        }

        private void WatchTimerTickedHandler(object sender, EventArgs e)
        {
            if (_selectedAppHandles.Contains(NativeHelper.GetForegroundWindow()))
            {
                _tokeiService.SwitchToJixue();
            }
            else
            {
                _tokeiService.SwitchToMoyu();
            }
        }

        private void ExecuteLoad(FrameworkElement fe)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(MoyuView));

            if (fe is Window win)
            {
                UpdateMonitorName(win);
            }
        }

        private void ExecutePrepareForTrayPopupOpening(TrayPopupOpenEventArgs args)
        {
            (TrayScalingCenterX, TrayScalingCenterY) = args.PopupDirection switch
            {
                Direction.LeftUp => (args.ElementSize.Width, args.ElementSize.Height),
                Direction.LeftDown => (args.ElementSize.Width, 0),
                Direction.RightUp => (0, args.ElementSize.Height),
                Direction.RightDown => (0, 0),
                _ => (TrayScalingCenterX, TrayScalingCenterY)
            };
        }

        private void ExecuteSwitchJudgementMode(JudgmentMode? mode)
        {
            if (!mode.HasValue)
            {
                return;
            }

            _tokeiService.Pause();
            _windowCheckTimer?.Stop();

            UpdateJudgmentMode(mode.Value);

            if (mode == JudgmentMode.MouseListening)
            {
                _tokeiService.Resume();

                UpdateTokeiMode(TokeiMode.Moyu);
            }
        }

        private void ExecuteSwitchTokeiMode()
        {
            UpdateTokeiMode(Mode switch
            {
                TokeiMode.Moyu => TokeiMode.Jixue,
                _ => TokeiMode.Moyu
            });
        }

        private async void ExecuteStartCapturingApplication()
        {
            if (Interlocked.CompareExchange(ref _dialogLock, 1, 0) != 0)
            {
                return;
            }

            try
            {
                _tokeiService.Pause();

                var appList = await RefreshApplicationList();

                if (appList?.Count is not > 0)
                {
                    _dialogService.ShowToast("不是哥们儿", "啥也没开啊？咋工作呢？");
                    return;
                }

                var data = new ApplicationSelection()
                {
                    CurrentAvailableApplications = appList,
                    SelectedApplicationHandles = _selectedAppHandles
                };

                var dialogParameter = new DialogParameters
                {
                    { DialogParameterKeys.Title, "请选择你的英雄" },
                    { DialogParameterKeys.NavigationTargetView, ViewNameKeys.WorkingAppSelector },
                    { DialogParameterKeys.PassingObject, data }
                };

                _dialogService.ShowDialog(ViewNameKeys.ConfirmCheckDialog, dialogParameter, result =>
                {
                    if (result.Result != ButtonResult.OK)
                    {
                        return;
                    }

                    if (result.Parameters.TryGetValue<ApplicationSelection>(DialogParameterKeys.PassingObject, out var list))
                    {
                        _selectedAppHandles = list.SelectedApplicationHandles;
                    }

                    if (_selectedAppHandles is not null)
                    {
                        _tokeiService.Resume();
                        _windowCheckTimer.Start();
                    }
                });
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("Oops", $"获取程序列表出事了QAQ（{ex.Message}）");
            }
            finally
            {
                Interlocked.Exchange(ref _dialogLock, 0);
            }
        }

        private void ExecuteDragMoveCompleted(Window window)
        {
            UpdateMonitorName(window);
        }

        private void ExecuteReset()
        {
            _tokeiService.Reset();
        }

        private void ExecuteExitApplication()
        {
            _eventAggregator.GetEvent<ExitApplicationEvent>().Publish();
        }

        private async void ExecuteRender(FrameworkElement fe)
        {
            if (Interlocked.CompareExchange(ref _renderLock, 1, 0) != 0)
            {
                return;
            }

            try
            {
                var drawing = new DrawingVisual();
                var dpi = VisualTreeHelper.GetDpi(fe);

                int usedWidth = (int)fe.ActualWidth, usedHeight = (int)fe.ActualHeight;
                var moyuBitmap = new RenderTargetBitmap(usedWidth, usedHeight, 96, 96, PixelFormats.Default);
                var jixueBitmap = new RenderTargetBitmap(usedWidth, usedHeight, 96, 96, PixelFormats.Default);
                var finalBitmap = new RenderTargetBitmap(usedWidth, usedHeight * 2, 96, 96, PixelFormats.Default);

                _tokeiService.Pause();
                var currentMode = _tokeiService.CurrentMode;

                using (var ctx = drawing.RenderOpen())
                {
                    _tokeiService.SwitchToMoyu();
                    await Dispatcher.Yield(DispatcherPriority.Background);

                    moyuBitmap.Render(fe);

                    ctx.DrawImage(moyuBitmap, new(new Point(0, 0), new Size(usedWidth, usedHeight)));

                    _tokeiService.SwitchToJixue();
                    await Dispatcher.Yield(DispatcherPriority.Background);

                    jixueBitmap.Render(fe);

                    ctx.DrawImage(jixueBitmap, new(new Point(0, usedHeight), new Size(usedWidth, usedHeight)));
                }

                finalBitmap.Render(drawing);

                if (currentMode == TokeiMode.Moyu)
                {
                    _tokeiService.SwitchToMoyu();
                }

                Clipboard.SetImage(finalBitmap);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("Oops", $"设置结果失败了（{ex.Message}）");
            }
            finally
            {
                Interlocked.Exchange(ref _renderLock, 0);
            }
        }
    }
}
