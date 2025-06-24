using Microsoft.Xaml.Behaviors;
using MoyuTokei.Common.Extensions;
using MoyuTokei.Core.Interop;
using System.Windows;

namespace MoyuTokei.Behaviors
{
    public class WindowFunctionSwitchBehavior : Behavior<Window>
    {
        public enum Functions
        {
            Nothing,
            BlurClientArea,
            ClickThrough
        }

        public Functions Function
        {
            get => (Functions)GetValue(FunctionProperty);
            set => SetValue(FunctionProperty, value);
        }

        public static readonly DependencyProperty FunctionProperty =
            DependencyProperty.Register(nameof(Function), typeof(Functions), typeof(WindowFunctionSwitchBehavior),
              new FrameworkPropertyMetadata(Functions.Nothing));

        public bool Enable
        {
            get => (bool)GetValue(EnableProperty);
            set => SetValue(EnableProperty, value);
        }

        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.Register(nameof(Enable), typeof(bool), typeof(WindowFunctionSwitchBehavior),
              new FrameworkPropertyMetadata(default(bool), OnEnableChanged));

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WindowFunctionSwitchBehavior self || e.NewValue is not bool enable)
            {
                return;
            }

            if (self.Function == Functions.Nothing)
            {
                return;
            }

            var handle = self.AssociatedObject.GetHandle();

            switch (self.Function)
            {
                case Functions.ClickThrough:
                    NativeHelper.SetWindowClickThrough(handle, enable);
                    break;
                case Functions.BlurClientArea:
                    NativeHelper.SetWindowBlur(handle, enable);
                    break;
            }
        }
    }
}