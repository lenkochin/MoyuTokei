using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigTargetMode
    {
        public DisplayConfigVideoSignalInfo TargetVideoSignalInfo;
    }
}
