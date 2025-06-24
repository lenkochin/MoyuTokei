using MoyuTokei.Common;
using MoyuTokei.Services.Interfaces;

namespace MoyuTokei.Services
{
    public class DialogCloseCheckService : IDialogCloseCheckService
    {
        private ICloseCheckingDialog? _dialog;

        public ICloseCheckingDialog? GetDialog()
        {
            return _dialog;
        }

        public void RegisterCloseCheckingDialog(ICloseCheckingDialog dialog)
        {
            _dialog = dialog;
        }
    }
}
