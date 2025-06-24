using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfig2DRegion
    {
        public uint CX;
        public uint CY;
    }
}