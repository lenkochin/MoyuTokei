using MoyuTokei.Common.Events;
using MoyuTokei.Services;
using MoyuTokei.Services.Interfaces;
using MoyuTokei.Views;
using MoyuTokei.Views.Dialog;
using Prism.Events;
using Prism.Ioc;
using System.Text;
using System.Windows;

namespace MoyuTokei
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITokeiService, TokeiService>();
            containerRegistry.Register<IDialogCloseCheckService, DialogCloseCheckService>();

            containerRegistry.RegisterForNavigation<MoyuView>();
            containerRegistry.RegisterForNavigation<WorkingAppSelectorView>();
            containerRegistry.RegisterForNavigation<MessageView>();

            containerRegistry.RegisterDialog<ConfirmCheckDialogView>();
            containerRegistry.RegisterDialog<ToastDialogView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            base.OnStartup(e);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Container.Resolve<IEventAggregator>().GetEvent<ExitApplicationEvent>().Subscribe(ExitApplication);
        }

        private void ExitApplication()
        {
            Shutdown();
        }
    }
}
