using System;

namespace CreateWindowHookLib.Win32.Model
{
    [Flags]
    public enum WindowStyles : uint
    {
        WsOverlapped = 0x00000000,
        WsPopup = 0x80000000,
        WsChild = 0x40000000,
        WsMinimize = 0x20000000,
        WsVisible = 0x10000000,
        WsDisabled = 0x08000000,
        WsClipsiblings = 0x04000000,
        WsClipChildren = 0x02000000,
        WsMaximize = 0x01000000,
        WsBorder = 0x00800000,
        WsDlgFrame = 0x00400000,
        WsVScroll = 0x00200000,
        WsHScroll = 0x00100000,
        WsSysmenu = 0x00080000,
        WsThickFrame = 0x00040000,
        WsGroup = 0x00020000,
        WsTabstop = 0x00010000,

        WsMinimizeBox = 0x00020000,
        WsMaximizeBox = 0x00010000,

        WsCaption = WsBorder | WsDlgFrame,
        WsTiled = WsOverlapped,
        WsIconic = WsMinimize,
        WsSizeBox = WsThickFrame,
        WsTiledWindow = WsOverlappedWindow,

        WsOverlappedWindow = WsOverlapped | WsCaption | WsSysmenu | WsThickFrame | WsMinimizeBox | WsMaximizeBox,
        WsPopupWindow = WsPopup | WsBorder | WsSysmenu,
        WsChildWindow = WsChild,

        //Extended Window Styles

        WsExDlgModalFrame = 0x00000001,
        WsExNoParentNotify = 0x00000004,
        WsExTopmost = 0x00000008,
        WsExAcceptFiles = 0x00000010,
        WsExTransparent = 0x00000020,

        //#if(WINVER >= 0x0400)

        WsExMdiChild = 0x00000040,
        WsExToolWindow = 0x00000080,
        WsExWindowEdge = 0x00000100,
        WsExClientEdge = 0x00000200,
        WsExContextHelp = 0x00000400,

        WsExRight = 0x00001000,
        WsExLeft = 0x00000000,
        WsExRtlReading = 0x00002000,
        WsExLtrReading = 0x00000000,
        WsExLeftScrollbar = 0x00004000,
        WsExRightScrollbar = 0x00000000,

        WsExControlparent = 0x00010000,
        WsExStaticEdge = 0x00020000,
        WsExAppWindow = 0x00040000,

        WsExOverlappedWindow = WsExWindowEdge | WsExClientEdge,
        WsExPaletteWindow = WsExWindowEdge | WsExToolWindow | WsExTopmost,
        //#endif /* WINVER >= 0x0400 */

        //#if(WIN32WINNT >= 0x0500)

        WsExLayered = 0x00080000,
        //#endif /* WIN32WINNT >= 0x0500 */

        //#if(WINVER >= 0x0500)

        WsExNoInheritLayout = 0x00100000, // Disable inheritence of mirroring by children
        WsExLayoutRtl = 0x00400000, // Right to left mirroring
        //#endif /* WINVER >= 0x0500 */

        //#if(WIN32WINNT >= 0x0500)

        WsExComposited = 0x02000000,

        WsExNoActivate = 0x08000000
        //#endif /* WIN32WINNT >= 0x0500 */
    }
}