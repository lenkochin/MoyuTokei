using Microsoft.Xaml.Behaviors;
using MoyuTokei.Common;
using MoyuTokei.Common.Extensions;
using MoyuTokei.Helpers;
using MoyuTokei.Core.Interop;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MoyuTokei.Behaviors
{
    public class SnapToCornerAction : TriggerAction<FrameworkElement>
    {
        public Corner SnapTo
        {
            get => (Corner)GetValue(SnapToProperty);
            set => SetValue(SnapToProperty, value);
        }

        public static readonly DependencyProperty SnapToProperty =
            DependencyProperty.Register(nameof(SnapTo), typeof(Corner), typeof(SnapToCornerAction),
              new FrameworkPropertyMetadata(default(Corner)));

        public bool UseAnimation
        {
            get => (bool)GetValue(UseAnimationProperty);
            set => SetValue(UseAnimationProperty, value);
        }

        public static readonly DependencyProperty UseAnimationProperty =
            DependencyProperty.Register(nameof(UseAnimation), typeof(bool), typeof(SnapToCornerAction),
              new FrameworkPropertyMetadata(true));

        protected override void Invoke(object parameter)
        {
            var win = Window.GetWindow(AssociatedObject);

            if (win is null)
            {
                return;
            }

            var monitorInfo = NativeHelper.GetMonitorFromWindowHandle(win.GetHandle());

            if (monitorInfo is null)
            {
                return;
            }

            var bounding = WindowHelper.GetMonitorRectFromWindow(win, true);

            var (left, top) = SnapTo switch
            {
                Corner.LeftTop => (bounding.Left, bounding.Top),
                Corner.RightTop => (bounding.Right - win.ActualWidth, bounding.Top),
                Corner.RightBottom => (bounding.Right - win.ActualWidth, bounding.Bottom - win.ActualHeight),
                Corner.LeftBottom => (bounding.Left, bounding.Bottom - win.ActualHeight),
                _ => (0, 0)
            };

            if (UseAnimation)
            {
                WindowHelper.AnimateWindowTo(win, left, top, new Duration(TimeSpan.FromSeconds(0.8)));
            }
            else
            {
                win.Left = left;
                win.Top = top;
            }
        }
    }
}
