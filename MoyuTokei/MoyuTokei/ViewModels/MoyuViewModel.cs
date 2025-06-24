using MoyuTokei.Common;
using MoyuTokei.Common.Events;
using MoyuTokei.Common.Events.Payloads;
using MoyuTokei.Core.Mvvm;
using MoyuTokei.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace MoyuTokei.ViewModels
{
    internal class MoyuViewModel : ViewModelBase
    {
        private readonly ITokeiService _tokeiService;
        private readonly IEventAggregator _eventAggregator;

        private TimeSpan _duration;
        private JudgmentMode _mode;

        public ICommand LoadedCommand { get; private set; }

        public ICommand MouseEnterCommand { get; private set; }

        public ICommand MouseLeaveCommand { get; private set; }

        public string ModeDescription => _tokeiService.CurrentMode switch
        {
            TokeiMode.Jixue => "鸡血",
            _ => "摸鱼"
        };

        public TokeiMode Mode => _tokeiService.CurrentMode;

        public JudgmentMode JudgmentMode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        public TimeSpan Duration
        {
            get => _duration;
            private set => SetProperty(ref _duration, value);
        }

        public MoyuViewModel(ITokeiService tokeiService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _tokeiService = tokeiService;
            _tokeiService.TimerTicked += TokeiTickedHandler;

            SetupCommands();
            RegisterEventListeners();
        }

        private void SetupCommands()
        {
            LoadedCommand = new DelegateCommand(ExecuteLoaded);
            MouseEnterCommand = new DelegateCommand(ExecuteMouseEnter);
            MouseLeaveCommand = new DelegateCommand(ExecuteMouseLeave);
        }

        private void RegisterEventListeners()
        {
            _eventAggregator.GetEvent<TokeiModeChangedEvent>().Subscribe(TokeiModeChangedHandler);
            _eventAggregator.GetEvent<JudgmentModeChangedEvent>().Subscribe(JudgmentModeChangedHandler);
        }

        private void TokeiTickedHandler(object sender, TimerTickedEventArgs e)
        {
            Duration = e.Duration;
        }

        private void TokeiModeChangedHandler(TokeiModeChangedPayload e)
        {
            RaisePropertyChanged(nameof(ModeDescription));
            RaisePropertyChanged(nameof(Mode));
        }

        private void JudgmentModeChangedHandler(JudgmentModeChangedPayload e)
        {
            JudgmentMode = e.CurrentMode;
        }

        private void ExecuteLoaded()
        {
            _tokeiService.Resume();
        }

        private void ExecuteMouseEnter()
        {
            if (JudgmentMode is not JudgmentMode.MouseListening)
            {
                return;
            }

            _tokeiService.SwitchToJixue();
        }

        private void ExecuteMouseLeave()
        {
            if (JudgmentMode is not JudgmentMode.MouseListening)
            {
                return;
            }

            _tokeiService.SwitchToMoyu();
        }
    }
}
