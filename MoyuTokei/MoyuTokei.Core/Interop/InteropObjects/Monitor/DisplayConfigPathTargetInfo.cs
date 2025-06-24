using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigPathTargetInfo
    {
        public Luid AdapterId;
        public uint Id;
        public uint ModeInfoIdx;
        public DisplayConfigVideoOutputTechnology outputTechnology;
        public DisplayConfigRotation Rotation;
        public DisplayConfigScaling Scaling;
        public DisplayConfigRational RefreshRate;
        public DisplayConfigScanlineOrdering ScanLineOrdering;
        public bool TargetAvailable;
        public uint StatusFlags;
    }
}
