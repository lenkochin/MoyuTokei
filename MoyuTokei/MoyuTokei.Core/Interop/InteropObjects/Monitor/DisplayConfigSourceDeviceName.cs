using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DisplayConfigSourceDeviceName
    {
        public DisplayConfigDeviceInfoHeader Header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ViewGdiDeviceName;
    }
}
