using Microsoft.Xaml.Behaviors;
using MoyuTokei.Common;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace MoyuTokei.Behaviors
{
    public class ScopedRegionManagerRegisterBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Register();
        }

        private void Register()
        {
            if (AssociatedObject.DataContext is not IScopedRegionManagerAware srma)
            {
                return;
            }

            var globalRegionManager = ContainerLocator.Container.Resolve<IRegionManager>();

            if (globalRegionManager is null)
            {
                return;
            }

            var newRegionManager = globalRegionManager.CreateRegionManager();

            RegionManager.SetRegionManager(AssociatedObject, newRegionManager);
            srma.ScopedRegionManager = newRegionManager;
        }
    }
}
