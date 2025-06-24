using MoyuTokei.Core.Interop.IntermidiateObjects;
using MoyuTokei.Core.Interop.InteropObjects;
using MoyuTokei.Core.Interop.InteropObjects.Monitor;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using UNM = MoyuTokei.Core.Interop.UnsafeNativeMethods;

namespace MoyuTokei.Core.Interop
{
    public static class NativeHelper
    {
        public static IntPtr GetWindowExStyle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                throw new ArgumentException($"{hWnd} cannot be empty.", nameof(hWnd));
            }

            return UNM.GetWindowLongPtr(hWnd, ConstantValues.GWL_EXSTYLE);
        }

        public static void SetWindowExStyle(IntPtr hWnd, IntPtr newStyle)
        {
            if (hWnd == IntPtr.Zero)
            {
                throw new ArgumentException($"{hWnd} cannot be empty.", nameof(hWnd));
            }

            UNM.SetWindowLongPtr(hWnd, ConstantValues.GWL_EXSTYLE, newStyle);
        }

        /// <summary>
        /// Make client area blur. DO NOT CALL THIS ON VERSIONS EARLIER THAN WINDOWS 10.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="enableBlur">Enable blur or not.</param>
        public static void SetWindowBlur(IntPtr hWnd, bool enableBlur)
        {
            AccentPolicy accent = new()
            {
                AccentState = enableBlur ? 3 : 0
            };

            var data = new WindowCompositionAttributeData()
            {
                Attribute = 19,
                Size = (uint)Marshal.SizeOf(accent)
            };

            var accentPtr = Marshal.AllocHGlobal((int)data.Size);

            Marshal.StructureToPtr(accent, accentPtr, false);

            data.Data = accentPtr;

            UNM.SetWindowCompositionAttribute(hWnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        public static MonitorInformation? GetMonitorFromWindowHandle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return null;
            }

            var monitorHandle = UNM.MonitorFromWindow(hWnd, ConstantValues.MONITOR_DEFAULTTIONNEAREST);

            if (monitorHandle == IntPtr.Zero)
            {
                return null;
            }

            MonitorInformation? result = null;

            UNM.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hdc, IntPtr clip, ref Rect rec, IntPtr dwData) =>
            {
                if (monitorHandle != IntPtr.Zero && hdc != monitorHandle)
                {
                    return true;
                }

                var info = new MonitorInfoEx()
                {
                    StructureSize = (uint)Marshal.SizeOf<MonitorInfoEx>()
                };

                if (!UNM.GetMonitorInfo(hdc, ref info))
                {
                    return false;
                }

                var devInfo = new DisplayDevice()
                {
                    StructureSize = (uint)(Marshal.SizeOf<DisplayDevice>())
                };

                //UNM.EnumDisplayDevices(null, 0, ref devInfo, ConstantValues.EDD_GET_DEVICE_INTERFACE_NAME);

                if (UNM.EnumDisplayDevices(devInfo.DeviceName, 0, ref devInfo, ConstantValues.EDD_GET_DEVICE_INTERFACE_NAME))
                {
                    var mi = new MonitorInformation()
                    {
                        MonitorName = devInfo.DeviceString,
                        DeviceName = info.DeviceName,
                        MonitorWorkArea = info.WorkArea,
                        MonitorRect = info.MonitorArea
                    };

                    result = mi;
                }

                return false;
            }, IntPtr.Zero);

            return result;
        }

        public static void SetWindowPosition(IntPtr hWnd, int x, int y) => SetWindowPosition(hWnd, IntPtr.Zero, x, y);

        public static void SetWindowPosition(IntPtr hWnd, IntPtr refhWndOrZOrder, int x, int y)
        {
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            UNM.SetWindowPos(hWnd, refhWndOrZOrder, x, y, 0, 0, ConstantValues.SWP_NOSIZE);
        }

        public static void SetWindowClickThrough(IntPtr hWnd, bool enableClickThrough)
        {
            var curLong = UNM.GetWindowLongPtr(hWnd, ConstantValues.GWL_EXSTYLE);

            var value2bSet = enableClickThrough switch
            {
                true => new IntPtr(curLong.ToInt32() | ConstantValues.WS_EX_TRANSPARENT),
                _ => new IntPtr(curLong.ToInt32() & ~ConstantValues.WS_EX_TRANSPARENT)
            };

            UNM.SetWindowLongPtr(hWnd, ConstantValues.GWL_EXSTYLE, value2bSet);
        }

        public static Rect? GetWindowSize(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return null;
            }

            UNM.GetWindowRect(hWnd, out var result);

            return result;
        }

        /// <summary>
        /// Get monitor friendly name by adapter name retrieved by <see cref="GetMonitorInfo"/>.
        /// </summary>
        /// <param name="gdiAdapterName">The adapter name of the monitor.</param>
        /// <returns></returns>
        public static string GetMonitorFriendlyName(string gdiAdapterName)
        {
            var finalResult = string.Empty;

            if (gdiAdapterName?.Length is not > 0)
            {
                return finalResult;
            }

            int result = UNM.GetDisplayConfigBufferSizes(ConstantValues.QDC_ONLY_ACTIVE_PATHS, out var pathCount, out var modeCount);
            
            if (result != 0)
            {
                return finalResult;
            }

            if (pathCount == 0)
            {
                return finalResult;
            }

            var paths = new DisplayConfigPathInfo[pathCount];
            var modes = new DisplayConfigModeInfo[modeCount];

            result = UNM.QueryDisplayConfig(ConstantValues.QDC_ONLY_ACTIVE_PATHS,
                                            ref pathCount,
                                            paths,
                                            ref modeCount,
                                            modes,
                                            IntPtr.Zero);

            if (result != 0)
            {
                return finalResult;
            }

            foreach (var path in paths)
            {
                DisplayConfigSourceDeviceName name = new();
                name.Header.Size = (uint)Marshal.SizeOf<DisplayConfigSourceDeviceName>();
                name.Header.AdapterId = path.SourceInfo.AdapterId;
                name.Header.Id = path.SourceInfo.Id;
                name.Header.Type = DisplayConfigDeviceInfoType.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME;

                result = UNM.DisplayConfigGetDeviceInfo(ref name);

                if (result != 0)
                {
                    break;
                }

                if (name.ViewGdiDeviceName != gdiAdapterName)
                {
                    continue;
                }

                // Matched
                DisplayConfigTargetDeviceName targetName = new();
                targetName.Header.Size = (uint)Marshal.SizeOf<DisplayConfigTargetDeviceName>();
                targetName.Header.AdapterId = path.TargetInfo.AdapterId;
                targetName.Header.Id = path.TargetInfo.Id;
                targetName.Header.Type = DisplayConfigDeviceInfoType.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;

                result = UNM.DisplayConfigGetDeviceInfo(ref targetName);

                if (result != 0)
                {
                    break;
                }

                finalResult = targetName.MonitorFriendlyDeviceName;
            }

            return finalResult;
        }

        public static string GetWindowCaption(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                return string.Empty;
            }

            StringBuilder result = new(100);

            _ = UNM.GetWindowText(hWnd, result, 100);

            return result.ToString();
        }

        public static async Task<List<(string Title, IntPtr Handle)>> GetVisibleWindowsAsync(CancellationToken token)
        {
            List<(string Title, IntPtr Handle)> result = [];

            await Task.Run(() => UNM.EnumWindows((hWnd, lParam) =>
            {
                if (token.IsCancellationRequested)
                {
                    return false;
                }

                string caption = GetWindowCaption(hWnd);

                if (string.IsNullOrEmpty(caption) || !UNM.IsWindowVisible(hWnd))
                {
                    return true;
                }

                if (token.IsCancellationRequested)
                {
                    return false;
                }

                IntPtr windowLong = UNM.GetWindowLongPtr(hWnd, ConstantValues.GWL_EXSTYLE);

                var info = new TitleBarInfo()
                {
                    StructureSize = Marshal.SizeOf(typeof(TitleBarInfo))
                };

                if (!UNM.GetTitleBarInfo(hWnd, ref info))
                {
                    return true;
                }

                if ((windowLong.ToInt32() & ConstantValues.WS_EX_NOREDIRECTIONBITMAP) == 0)
                {
                    result.Add((caption, hWnd));
                }

                return true;
            }, IntPtr.Zero), token);

            return result;
        }

        public static IntPtr GetForegroundWindow() => UNM.GetForegroundWindow();
    }
}