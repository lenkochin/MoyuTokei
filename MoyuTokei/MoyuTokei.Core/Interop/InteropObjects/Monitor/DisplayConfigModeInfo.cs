using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigModeInfo
    {
        public DisplayConfigModeInfoType InfoType;
        public uint Id;
        public Luid AdapterId;
        public DisplayConfigTargetMode ModeInfo;
    }
}