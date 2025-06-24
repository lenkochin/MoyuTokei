namespace MoyuTokei.Core.Interop
{
    public static class ConstantValues
    {
        // Window style types
        public const int GWL_EXSTYLE = -20;
        public const int GWL_STYLE = -16;

        // Window EX styles
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_COMPOSITED = 0x02000000;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_LAYOUTRTL = 0x00400000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_WINDOWEDGE = 0x00000100;

        // Monitor information related
        public const int MONITOR_DEFAULTTIONNEAREST = 2;
        public const int MONITOR_DEFAULTTIONPRIMARY = 1;
        public const int EDD_GET_DEVICE_INTERFACE_NAME = 0x1;
        public const int EDS_ROTATEDMODE = 0x4;

        // Window position related
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_SHOWWINDOW = 0x0040;

        // Monitor counts
        public const int SM_CMONITORS = 80;

        public const int QDC_ONLY_ACTIVE_PATHS = 0x00000002;
        public const int DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2;
    }
}
