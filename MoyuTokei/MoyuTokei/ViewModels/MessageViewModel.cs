using MoyuTokei.Core.Mvvm;
using MoyuTokei.Keys;
using Prism.Regions;

namespace MoyuTokei.ViewModels
{
    internal class MessageViewModel : ViewModelBase, INavigationAware
    {
        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<string>(DialogParameterKeys.PassingObject, out var message))
            {
                Message = message;
            }
        }
    }
}
