using Prism.Services.Dialogs;

namespace MoyuTokei.Common
{
    public interface ICloseCheckingDialog
    {
        bool CanConfirmAndClose();

        bool CanDiscardAndClose();

        void CloseTriggering(DialogResult result);
    }
}