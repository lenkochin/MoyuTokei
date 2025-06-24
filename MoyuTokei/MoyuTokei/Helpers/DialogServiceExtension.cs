using MoyuTokei.Keys;
using Prism.Services.Dialogs;

namespace MoyuTokei.Helpers
{
    internal static class DialogServiceExtension
    {
        public static void ShowMessage(this IDialogService dialogService, string title, string message)
        {
            // Use ConfirmCheckDialogView as template.
            var parameter = new DialogParameters
            {
                { DialogParameterKeys.Title, title },
                { DialogParameterKeys.PassingObject, message },
                { DialogParameterKeys.NavigationTargetView, ViewNameKeys.MessageHost },
                { DialogParameterKeys.CancelVisible, false }
            };

            dialogService.ShowDialog(ViewNameKeys.ConfirmCheckDialog, parameter, null);
        }

        public static void ShowToast(this IDialogService dialogService, string title, string message)
        {
            // Use ToastDialogView as template.
            var parameter = new DialogParameters
            {
                { DialogParameterKeys.Title, title },
                { DialogParameterKeys.PassingObject, message },
                { DialogParameterKeys.NavigationTargetView, ViewNameKeys.MessageHost }
            };

            dialogService.ShowDialog(ViewNameKeys.ToastDialog, parameter, null);
        }
    }
}
