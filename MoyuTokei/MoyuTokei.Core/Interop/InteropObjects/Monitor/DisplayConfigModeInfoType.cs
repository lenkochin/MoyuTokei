namespace MoyuTokei.Core.Interop.InteropObjects.Monitor
{
    public enum DisplayConfigModeInfoType : uint
    {
        DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE = 1,
        DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2,
        DISPLAYCONFIG_MODE_INFO_TYPE_DESKTOP_IMAGE = 3, // Windows 8 and later
        DISPLAYCONFIG_MODE_INFO_TYPE_FORCE_UINT32 = 0xFFFFFFFF
    }
}