using MoyuTokei.Common;

namespace MoyuTokei.Services.Interfaces
{
    public interface IDialogCloseCheckService
    {
        ICloseCheckingDialog? GetDialog();

        void RegisterCloseCheckingDialog(ICloseCheckingDialog dialog);
    }
}