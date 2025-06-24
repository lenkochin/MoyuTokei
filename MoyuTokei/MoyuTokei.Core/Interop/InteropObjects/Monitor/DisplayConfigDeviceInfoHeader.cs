using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigDeviceInfoHeader
    {
        public DisplayConfigDeviceInfoType Type;
        public uint Size;
        public Luid AdapterId;
        public uint Id;
    }
}
