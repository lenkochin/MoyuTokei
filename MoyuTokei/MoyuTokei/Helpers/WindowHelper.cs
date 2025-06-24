using MoyuTokei.Common.Extensions;
using MoyuTokei.Core.Interop;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MoyuTokei.Helpers
{
    public static class WindowHelper
    {
        private static (double Left, double Top) CalculateWindowSnapTarget(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            var transformed = GetMonitorRectFromWindow(window, true);

            double left = Math.Min(Math.Max(window.Left, transformed.Left), transformed.Right - window.ActualWidth);
            double top = Math.Min(Math.Max(window.Top, transformed.Top), transformed.Bottom - window.ActualHeight);

            return (left, top);
        }

        private static void SnapWindowToEdgeTeleport(Window window)
        {
            var (left, top) = CalculateWindowSnapTarget(window);

            window.Left = left;
            window.Top = top;
        }

        private static void SnapWindowToEdgeAnimate(Window window)
        {
            var (left, top) = CalculateWindowSnapTarget(window);

            AnimateWindowTo(window, left, top);
        }

        /// <summary>
        /// Snap the window to screen edge if it is outbound of the work area.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="useAnimation"></param>
        public static void SnapWindowToEdge(Window window, bool useAnimation)
        {
            if (!useAnimation)
            {
                SnapWindowToEdgeTeleport(window);
            }
            else
            {
                SnapWindowToEdgeAnimate(window);
            }
        }

        public static void AnimateWindowTo(Window window, double x, double y, Duration? aniDuration = null)
        {
            var sb = new Storyboard() { FillBehavior = FillBehavior.Stop };
            var duration = aniDuration ?? new Duration(TimeSpan.FromSeconds(0.3));

            var easingFunction = new PowerEase() { Power = 3 };

            var dAniLeft = new DoubleAnimation(window.Left, x, duration) { EasingFunction = easingFunction };
            var dAniTop = new DoubleAnimation(window.Top, y, duration) { EasingFunction = easingFunction };

            sb.Children.Add(dAniLeft);
            sb.Children.Add(dAniTop);

            Storyboard.SetTarget(sb, window);
            Storyboard.SetTargetProperty(dAniLeft, new PropertyPath(Window.LeftProperty));
            Storyboard.SetTargetProperty(dAniTop, new PropertyPath(Window.TopProperty));

            sb.Begin();
        }

        /// <summary>
        /// Retrieve the bounding rectangle of the screen containing the window.
        /// </summary>
        /// <param name="window">The reference window.</param>
        /// <param name="getWorkArea"><see langword="true"/> for the screen working area, otherwise the whole screen area.</param>
        /// <returns></returns>
        public static Rect GetMonitorRectFromWindow(Window window, bool getWorkArea)
        {
            var monitorInfo = NativeHelper.GetMonitorFromWindowHandle(window.GetHandle());

            if (monitorInfo is null)
            {
                return Rect.Empty;
            }

            var rectInPixel = getWorkArea ? monitorInfo.Value.MonitorWorkArea : monitorInfo.Value.MonitorRect;

            var transform = new MatrixTransform(PresentationSource.FromVisual(window).CompositionTarget.TransformFromDevice);
            // The transformed screen rect.
            var rectInWpfUnit = transform.TransformBounds(new Rect(rectInPixel.Left,
                                                                            rectInPixel.Top,
                                                                            rectInPixel.Right - rectInPixel.Left,
                                                                            rectInPixel.Bottom - rectInPixel.Top));

            return rectInWpfUnit;
        }

        public static string GetMonitorFriendlyNameFromWindow(Window window)
        {
            if (window is null)
            {
                return string.Empty;
            }

            var monitorInfo = NativeHelper.GetMonitorFromWindowHandle(window.GetHandle());

            if (monitorInfo is null)
            {
                return string.Empty;
            }

            return NativeHelper.GetMonitorFriendlyName(monitorInfo.Value.DeviceName);
        }
    }
}