using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DisplayConfigTargetDeviceNameFlags
    {
        public uint Value;

        public readonly bool FriendlyNameFromEdid => (Value & 0x1) != 0;
        public readonly bool FriendlyNameForced => (Value & 0x2) != 0;
        public readonly bool EdidIdsValid => (Value & 0x4) != 0;
    }
}
