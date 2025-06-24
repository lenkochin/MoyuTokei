using MoyuTokei.Core;
using MoyuTokei.Core.Mvvm;
using MoyuTokei.Keys;
using MoyuTokei.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Input;

namespace MoyuTokei.ViewModels.Dialog
{
    internal class ConfirmCheckDialogViewModel : DialogBaseViewModel
    {
        private readonly IDialogCloseCheckService _dlgCloseCheckSvr;

        private bool _confirmClose;
        private bool _confirmVisible, _cancelVisible;

        public bool ConfirmVisible
        {
            get => _confirmVisible;
            set => SetProperty(ref _confirmVisible, value);
        }

        public bool CancelVisible
        {
            get => _cancelVisible;
            set => SetProperty(ref _cancelVisible, value);
        }

        public ICommand ConfirmCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ConfirmCheckDialogViewModel(IDialogCloseCheckService dialogCloseCheckService)
            :base(dialogCloseCheckService)
        {
            _dlgCloseCheckSvr = dialogCloseCheckService;

            SetupCommands();
        }

        private void SetupCommands()
        {
            ConfirmCommand = new DelegateCommand(ExecuteConfirm);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        private void ExecuteConfirm()
        {
            _confirmClose = true;

            if (!CanCloseDialog())
            {
                return;
            }

            RequestCloseDialog(ButtonResult.OK);
        }

        private void ExecuteCancel()
        {
            _confirmClose = false;

            if (!CanCloseDialog())
            {
                return;
            }

            RequestCloseDialog(ButtonResult.Cancel);
        }

        public override bool CanCloseDialog()
        {
            var closeCheck = _dlgCloseCheckSvr.GetDialog();

            if (closeCheck is not null)
            {
                return _confirmClose ? closeCheck.CanConfirmAndClose() : closeCheck.CanDiscardAndClose();
            }

            return true;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.ContainsKey(DialogParameterKeys.NavigationTargetView))
            {
                return;
            }

            Title = parameters.GetValue<string>(DialogParameterKeys.Title);
            ConfirmVisible = parameters.GetValue<bool?>(DialogParameterKeys.ConfirmVisible) ?? true;
            CancelVisible = parameters.GetValue<bool?>(DialogParameterKeys.CancelVisible) ?? true;

            var naviParams = new NavigationParameters
            {
                { DialogParameterKeys.CloseCheckService, _dlgCloseCheckSvr },
                { DialogParameterKeys.PassingObject, parameters.GetValue<object>(DialogParameterKeys.PassingObject) }
            };
            var targetName = parameters.GetValue<string>(DialogParameterKeys.NavigationTargetView);

            ScopedRegionManager?.RequestNavigate(RegionNames.DialogContentRegion, targetName, naviParams);
        }
    }
}