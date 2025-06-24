using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigVideoSignalInfo
    {
        public ulong PixelRate;
        public DisplayConfigRational HorizontalSyncFreq;
        public DisplayConfigRational VerticalSyncFreq;
        public DisplayConfig2DRegion ActiveSize;
        public DisplayConfig2DRegion TotalSize;
        public uint VideoStandard;
        public uint ScanLineOrdering;
    }
}
