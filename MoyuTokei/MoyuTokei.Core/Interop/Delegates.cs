using MoyuTokei.Core.Interop.InteropObjects;
using System;

namespace MoyuTokei.Core.Interop
{
    public delegate bool EnumDisplayCallback(IntPtr hdc, IntPtr lprcClip, ref Rect rec, IntPtr dwData);
    public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);
}
