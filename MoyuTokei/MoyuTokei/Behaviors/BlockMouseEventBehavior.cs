using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace MoyuTokei.Behaviors
{
    public class BlockMouseEventBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not null)
            {
                AssociatedObject.MouseDown += PreviewMouseDownHandler;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is not null)
            {
                AssociatedObject.MouseDown -= PreviewMouseDownHandler;
            }
        }

        private void PreviewMouseDownHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}