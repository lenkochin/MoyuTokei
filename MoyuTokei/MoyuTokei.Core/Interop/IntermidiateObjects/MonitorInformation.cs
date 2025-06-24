using MoyuTokei.Core.Interop.InteropObjects;

namespace MoyuTokei.Core.Interop.IntermidiateObjects
{
    public struct MonitorInformation
    {
        public string MonitorName { get; internal set; }
        public string DeviceName { get; internal set; }
        public Rect MonitorRect { get; internal set; }
        public Rect MonitorWorkArea { get; internal set; }
    }
}
