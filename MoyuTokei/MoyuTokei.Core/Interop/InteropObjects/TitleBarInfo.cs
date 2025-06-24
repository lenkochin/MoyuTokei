using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects
{
    internal struct TitleBarInfo
    {
        public int StructureSize;
        public Rect TitleBarRect;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] ElementsState;
    }
}