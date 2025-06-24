using MoyuTokei.Core;
using MoyuTokei.Core.Mvvm;
using MoyuTokei.Keys;
using MoyuTokei.Services.Interfaces;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Timers;
using System.Windows.Threading;

namespace MoyuTokei.ViewModels.Dialog
{
    internal class ToastDialogViewModel(IDialogCloseCheckService dlgCloseCheckSvr) : DialogBaseViewModel(dlgCloseCheckSvr)
    {
        private void ScheduleClosing(TimeSpan? period)
        {
            period ??= TimeSpan.FromSeconds(3);

            var timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = period.Value };
            timer.Tick += (_, _) =>
            {
                RequestCloseDialog(ButtonResult.OK);
                timer.Stop();
            };
            timer.Start();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.ContainsKey(DialogParameterKeys.NavigationTargetView))
            {
                return;
            }

            Title = parameters.GetValue<string>(DialogParameterKeys.Title);

            var naviParams = new NavigationParameters
            {
                { DialogParameterKeys.PassingObject, parameters.GetValue<object>(DialogParameterKeys.PassingObject) }
            };
            var targetName = parameters.GetValue<string>(DialogParameterKeys.NavigationTargetView);

            ScopedRegionManager?.RequestNavigate(RegionNames.DialogContentRegion, targetName, naviParams);

            ScheduleClosing(parameters.GetValue<TimeSpan?>(DialogParameterKeys.Period));
        }
    }
}