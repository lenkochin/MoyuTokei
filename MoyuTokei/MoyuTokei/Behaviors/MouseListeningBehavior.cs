using Microsoft.Xaml.Behaviors;
using MoyuTokei.Common;
using System.Windows;
using System.Windows.Input;

namespace MoyuTokei.Behaviors
{
    public class MouseListeningBehavior : Behavior<FrameworkElement>
    {
        public JudgmentMode JudgmentMode
        {
            get => (JudgmentMode)GetValue(JudgmentModeProperty);
            set => SetValue(JudgmentModeProperty, value);
        }

        public static readonly DependencyProperty JudgmentModeProperty =
            DependencyProperty.Register(nameof(JudgmentMode), typeof(JudgmentMode), typeof(MouseListeningBehavior),
                new FrameworkPropertyMetadata(default(JudgmentMode), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnJudgmentModeChanged));

        public ICommand MouseEnterCommand
        {
            get => (ICommand)GetValue(MouseEnterCommandProperty);
            set => SetValue(MouseEnterCommandProperty, value);
        }

        public static readonly DependencyProperty MouseEnterCommandProperty =
            DependencyProperty.Register(nameof(MouseEnterCommand), typeof(ICommand), typeof(MouseListeningBehavior),
              new FrameworkPropertyMetadata(default(ICommand)));

        public ICommand MouseLeaveCommand
        {
            get => (ICommand)GetValue(MouseLeaveCommandProperty);
            set => SetValue(MouseLeaveCommandProperty, value);
        }

        public static readonly DependencyProperty MouseLeaveCommandProperty =
            DependencyProperty.Register(nameof(MouseLeaveCommand), typeof(ICommand), typeof(MouseListeningBehavior),
              new FrameworkPropertyMetadata(default(ICommand)));

        protected override void OnAttached()
        {
            base.OnAttached();

            if (JudgmentMode == JudgmentMode.MouseListening)
            {
                RegisterEvents();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            UnregisterEvents();
        }

        private static void OnJudgmentModeChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is not JudgmentMode jdgMode || dp is not MouseListeningBehavior self)
            {
                return;
            }

            self.UnregisterEvents();

            if (jdgMode == JudgmentMode.MouseListening)
            {
                self.RegisterEvents();
            }
        }

        private void RegisterEvents()
        {
            if (AssociatedObject is null)
            {
                return;
            }

            AssociatedObject.MouseEnter += MouseEnterHandler;
            AssociatedObject.MouseLeave += MouseLeaveHandler;
        }

        private void UnregisterEvents()
        {
            if (AssociatedObject is null)
            {
                return;
            }

            AssociatedObject.MouseEnter -= MouseEnterHandler;
            AssociatedObject.MouseLeave -= MouseLeaveHandler;
        }

        private void MouseLeaveHandler(object sender, MouseEventArgs e)
        {
            if (MouseLeaveCommand != null && MouseLeaveCommand.CanExecute(null))
            {
                MouseLeaveCommand.Execute(null);
            }
        }

        private void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            if (MouseEnterCommand != null && MouseEnterCommand.CanExecute(null))
            {
                MouseEnterCommand.Execute(null);
            }
        }
    }
}