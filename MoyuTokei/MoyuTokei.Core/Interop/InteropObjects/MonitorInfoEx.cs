using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorInfoEx
    {
        public uint StructureSize;
        public Rect MonitorArea;
        public Rect WorkArea;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
    }
}
