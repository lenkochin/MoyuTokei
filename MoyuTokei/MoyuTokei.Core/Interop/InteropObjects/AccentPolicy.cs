using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        public int AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }
}
