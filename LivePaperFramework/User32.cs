using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LivePaperFramework
{
    public class User32
    {
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [Flags]
        public enum SetWindowPosFlags : int
        {
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            SWP_ASYNCWINDOWPOS = 0x4000,
            ///     Prevents generation of the WM_SYNCPAINT message.
            SWP_DEFERERASE = 0x2000,
            ///     Draws a frame (defined in the window's class description) around the window.
            SWP_DRAWFRAME = 0x0020,
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            SWP_FRAMECHANGED = 0x0020,
            ///     Hides the window.
            SWP_HIDEWINDOW = 0x0080,
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            SWP_NOACTIVATE = 0x0010,
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            SWP_NOCOPYBITS = 0x0100,
            ///     Retains the current position (ignores X and Y parameters).
            SWP_NOMOVE = 0x0002,
            ///     Does not change the owner window's position in the Z order.
            SWP_NOOWNERZORDER = 0x0200,
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            SWP_NOREDRAW = 0x0008,
            ///     Same as the SWP_NOOWNERZORDER flag.
            SWP_NOREPOSITION = 0x0200,
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            SWP_NOSENDCHANGING = 0x0400,
            ///     Retains the current size (ignores the cx and cy parameters).
            SWP_NOSIZE = 0x0001,
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            SWP_NOZORDER = 0x0004,
            ///     Displays the window.
            SWP_SHOWWINDOW = 0x0040,
        }

        [Flags()]
        public enum DeviceContextValues : uint
        {
            Window = 0x00000001,
            Cache = 0x00000002,
            NoResetAttrs = 0x00000004,
            ClipChildren = 0x00000008,
            ClipSiblings = 0x00000010,
            ParentClip = 0x00000020,
            ExcludeRgn = 0x00000040,
            IntersectRgn = 0x00000080,
            ExcludeUpdate = 0x00000100,
            IntersectUpdate = 0x00000200,
            LockWindowUpdate = 0x00000400,
            Validate = 0x00200000,
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DeviceContextValues flags);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32")]
        public static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("user32")]
        public static extern bool IsIconic(IntPtr windowHandle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(int uAction, int uParam, ref int lpvParam, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        public static UInt32 SPIF_SENDWININICHANGE = 0x02;
        public static UInt32 SPI_SETDESKWALLPAPER = 20;
        public static UInt32 SPIF_UPDATEINIFILE = 0x1;
        public static UInt32 SPI_SETCLIENTAREAANIMATION = 0x1043;
    }
}
