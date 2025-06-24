using Microsoft.Xaml.Behaviors;
using MoyuTokei.Helpers;
using System.Windows;
using System.Windows.Input;

namespace MoyuTokei.Behaviors
{
    public class WindowDragMoveBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Snap to the edge of work area when the window outside of it.
        /// </summary>
        public bool SnapToEdge
        {
            get => (bool)GetValue(SnapToEdgeProperty);
            set => SetValue(SnapToEdgeProperty, value);
        }

        public static readonly DependencyProperty SnapToEdgeProperty =
            DependencyProperty.Register(nameof(SnapToEdge), typeof(bool), typeof(WindowDragMoveBehavior),
              new FrameworkPropertyMetadata(default(bool)));

        public ICommand DragMoveCompletedCommand
        {
            get => (ICommand)GetValue(DragMoveCompletedCommandProperty);
            set => SetValue(DragMoveCompletedCommandProperty, value);
        }

        public static readonly DependencyProperty DragMoveCompletedCommandProperty =
            DependencyProperty.Register(nameof(DragMoveCompletedCommand), typeof(ICommand), typeof(WindowDragMoveBehavior),
              new FrameworkPropertyMetadata(default(ICommand)));

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not null)
            {
                AssociatedObject.MouseLeftButtonDown += MouseMoveHandler;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is not null)
            {
                AssociatedObject.MouseLeftButtonDown -= MouseMoveHandler;
            }
        }

        private void MouseMoveHandler(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(AssociatedObject);

            if (window is null)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();

                if (DragMoveCompletedCommand?.CanExecute(window) is true)
                {
                    DragMoveCompletedCommand.Execute(window);
                }

                if (SnapToEdge)
                {
                    WindowHelper.SnapWindowToEdge(window, true);
                }
            }
        }
    }
}