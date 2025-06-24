using MoyuTokei.Common;
using MoyuTokei.Core.Mvvm;
using MoyuTokei.DataExchange;
using MoyuTokei.Keys;
using MoyuTokei.Services.Interfaces;
using MoyuTokei.ViewModels.ItemLevel;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoyuTokei.ViewModels
{
    internal class WorkingAppSelectorViewModel : ViewModelBase, INavigationAware, ICloseCheckingDialog
    {
        public ObservableCollection<DesktopAppInfoViewModel> ApplicationCollections { get; } = [];

        public bool CanConfirmAndClose() => true;

        public bool CanDiscardAndClose() => true;

        public void CloseTriggering(DialogResult result)
        {
            result.Parameters.Add(DialogParameterKeys.PassingObject, new ApplicationSelection()
            {
                SelectedApplicationHandles = [.. ApplicationCollections.Where(d => d.IsSelected).Select(d => d.AppHandle)]
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var service = navigationContext.Parameters.GetValue<IDialogCloseCheckService>(DialogParameterKeys.CloseCheckService);

            service?.RegisterCloseCheckingDialog(this);

            var ok = navigationContext.Parameters.TryGetValue<ApplicationSelection>(DialogParameterKeys.PassingObject, out var value);

            if (ok && value.CurrentAvailableApplications is not null)
            {
                foreach (var (title, handle) in value.CurrentAvailableApplications)
                {
                    ApplicationCollections.Add(new(title, handle) { IsSelected = value.SelectedApplicationHandles?.Contains(handle) ?? false });
                }
            }
        }
    }
}