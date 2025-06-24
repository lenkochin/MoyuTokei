using MoyuTokei.Core.Interop.InteropObjects;
using MoyuTokei.Core.Interop.InteropObjects.Monitor;
using System;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable SYSLIB1054

namespace MoyuTokei.Core.Interop
{
    internal static partial class UnsafeNativeMethods
    {
        /// <summary>
        /// Get the window style information.
        /// </summary>
        /// <param name="hWnd">The window handle</param>
        /// <param name="index">Parameter index</param>
        /// <returns>A pointer of parameter</returns>
        [LibraryImport(DllName.User32, EntryPoint = "GetWindowLongW")]
        internal static partial IntPtr GetWindowLongPtr(IntPtr hWnd, int index);

        /// <summary>
        /// Set the window style information.
        /// </summary>
        /// <param name="hWnd">The window handle</param>
        /// <param name="index">Parameter index</param>
        /// <param name="newLong">New parameter</param>
        /// <returns>A pointer of parameter</returns>
        [LibraryImport(DllName.User32, EntryPoint = "SetWindowLongW")]
        internal static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int index, IntPtr newLong);

        /// <summary>
        /// Undocumented API which can be used to make window client area blur.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [LibraryImport(DllName.User32)]
        internal static partial int SetWindowCompositionAttribute(IntPtr hWnd, ref WindowCompositionAttributeData data);

        /// <summary>
        /// Get the monitor information where the window placed at.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [LibraryImport(DllName.User32)]
        internal static partial IntPtr MonitorFromWindow(IntPtr hWnd, uint flags);

        [LibraryImport(DllName.User32)]
        internal static partial void EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumDisplayCallback callback, IntPtr dwData);

        /// <summary>
        /// Get the monitor information by handle.
        /// (LibraryImport requires custom marshalling for the type <see cref="MonitorInfoEx"/>. Such inconvenience for this tiny project.)
        /// </summary>
        /// <param name="hMonitor"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport(DllName.User32)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx info);

        [DllImport(DllName.User32)]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice displayDev, uint dwFlags);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint flags);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GetWindowRect(IntPtr hWnd, out Rect rect);

        [LibraryImport(DllName.User32)]
        internal static partial int GetDisplayConfigBufferSizes(uint flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        [DllImport(DllName.User32)]
        internal static extern int QueryDisplayConfig(uint flags,
                                                      ref uint numPathArrayElements,
                                                      [Out] DisplayConfigPathInfo[] pathArray,
                                                      ref uint numModeInfoArrayElements,
                                                      [Out] DisplayConfigModeInfo[] modeInfoArray,
                                                      IntPtr currentTopologyId);

        [DllImport(DllName.User32)]
        internal static extern int DisplayConfigGetDeviceInfo(ref DisplayConfigTargetDeviceName deviceName);

        [DllImport(DllName.User32)]
        internal static extern int DisplayConfigGetDeviceInfo(ref DisplayConfigSourceDeviceName deviceName);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool EnumWindows(EnumWindowsCallback callback, IntPtr lParam);

        [DllImport(DllName.User32, CharSet = CharSet.Unicode)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder caption, int maxCount);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool IsWindowVisible(IntPtr hWnd);

        [DllImport(DllName.User32)]
        internal static extern bool GetTitleBarInfo(IntPtr hWnd, ref TitleBarInfo info);

        [LibraryImport(DllName.User32)]
        internal static partial IntPtr GetForegroundWindow();
    }
}

#pragma warning restore SYSLIB1054