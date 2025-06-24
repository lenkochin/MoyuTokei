using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Luid
    {
        public uint LowPart;
        public int HighPart;
    }
}
