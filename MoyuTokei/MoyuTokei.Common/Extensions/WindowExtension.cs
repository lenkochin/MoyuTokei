using System.Windows;
using System.Windows.Interop;

namespace MoyuTokei.Common.Extensions
{
    public static class WindowExtension
    {
        public static IntPtr GetHandle(this Window win) => GetHandle(win, false);

        public static IntPtr GetHandle(this Window win, bool ensureCreated)
        {
            var interopHelper = new WindowInteropHelper(win);

            return ensureCreated ? interopHelper.EnsureHandle() : interopHelper.Handle;
        }
    }
}