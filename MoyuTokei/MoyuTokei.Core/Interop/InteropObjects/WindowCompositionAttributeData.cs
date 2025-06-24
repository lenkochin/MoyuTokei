using System;
using System.Runtime.InteropServices;

namespace MoyuTokei.Core.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public int Attribute;
        public IntPtr Data;
        public uint Size;
    }
}
