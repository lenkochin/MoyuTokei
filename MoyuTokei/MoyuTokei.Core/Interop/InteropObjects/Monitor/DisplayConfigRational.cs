using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigRational
    {
        public uint Numerator;
        public uint Denominator;
    }
}
