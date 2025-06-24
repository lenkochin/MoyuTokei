using MoyuTokei.Common;
using MoyuTokei.Services.Interfaces;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace MoyuTokei.Core.Mvvm
{
    public abstract class DialogBaseViewModel(IDialogCloseCheckService dlgCloseCheckSvr) : ViewModelBase, IDialogAware, IScopedRegionManagerAware
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref  _title, value);
        }

        public IRegionManager ScopedRegionManager { get; set; }

        public event Action<IDialogResult> RequestClose;

        private void CallCloseTriggering(DialogResult result)
        {
            dlgCloseCheckSvr?.GetDialog()?.CloseTriggering(result);
        }

        protected virtual void RequestCloseDialog(ButtonResult result)
        {
            var dlgResult = new DialogResult(result);

            CallCloseTriggering(dlgResult);
            RequestClose?.Invoke(dlgResult);
        }

        public virtual bool CanCloseDialog() => true;

        public virtual void OnDialogClosed() { }

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}