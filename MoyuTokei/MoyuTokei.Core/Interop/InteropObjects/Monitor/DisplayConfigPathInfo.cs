using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigPathInfo
    {
        public DisplayConfigPathSourceInfo SourceInfo;
        public DisplayConfigPathTargetInfo TargetInfo;
        public uint Flags;
    }
}
