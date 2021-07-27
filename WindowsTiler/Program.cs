using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Serilog;
using Serilog.Context;

namespace WindowsTiler
{
    public static class Api
    {
        /// <summary>
        ///     Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered
        ///     according to their appearance on the screen. The topmost window receives the highest rank and is the first window
        ///     in the Z order.
        ///     <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545%28v=vs.85%29.aspx for more information.</para>
        /// </summary>
        /// <param name="hWnd">C++ ( hWnd [in]. Type: HWND )<br />A handle to the window.</param>
        /// <param name="hWndInsertAfter">
        ///     C++ ( hWndInsertAfter [in, optional]. Type: HWND )<br />A handle to the window to precede the positioned window in
        ///     the Z order. This parameter must be a window handle or one of the following values.
        ///     <list type="table">
        ///     <itemheader>
        ///         <term>HWND placement</term><description>Window to precede placement</description>
        ///     </itemheader>
        ///     <item>
        ///         <term>HWND_BOTTOM ((HWND)1)</term>
        ///         <description>
        ///         Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost
        ///         window, the window loses its topmost status and is placed at the bottom of all other windows.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_NOTOPMOST ((HWND)-2)</term>
        ///         <description>
        ///         Places the window above all non-topmost windows (that is, behind all topmost windows). This
        ///         flag has no effect if the window is already a non-topmost window.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_TOP ((HWND)0)</term><description>Places the window at the top of the Z order.</description>
        ///     </item>
        ///     <item>
        ///         <term>HWND_TOPMOST ((HWND)-1)</term>
        ///         <description>
        ///         Places the window above all non-topmost windows. The window maintains its topmost position
        ///         even when it is deactivated.
        ///         </description>
        ///     </item>
        ///     </list>
        ///     <para>For more information about how this parameter is used, see the following Remarks section.</para>
        /// </param>
        /// <param name="X">C++ ( X [in]. Type: int )<br />The new position of the left side of the window, in client coordinates.</param>
        /// <param name="Y">C++ ( Y [in]. Type: int )<br />The new position of the top of the window, in client coordinates.</param>
        /// <param name="cx">C++ ( cx [in]. Type: int )<br />The new width of the window, in pixels.</param>
        /// <param name="cy">C++ ( cy [in]. Type: int )<br />The new height of the window, in pixels.</param>
        /// <param name="uFlags">
        ///     C++ ( uFlags [in]. Type: UINT )<br />The window sizing and positioning flags. This parameter can be a combination
        ///     of the following values.
        ///     <list type="table">
        ///     <itemheader>
        ///         <term>HWND sizing and positioning flags</term>
        ///         <description>Where to place and size window. Can be a combination of any</description>
        ///     </itemheader>
        ///     <item>
        ///         <term>SWP_ASYNCWINDOWPOS (0x4000)</term>
        ///         <description>
        ///         If the calling thread and the thread that owns the window are attached to different input
        ///         queues, the system posts the request to the thread that owns the window. This prevents the calling
        ///         thread from blocking its execution while other threads process the request.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_DEFERERASE (0x2000)</term>
        ///         <description>Prevents generation of the WM_SYNCPAINT message. </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_DRAWFRAME (0x0020)</term>
        ///         <description>Draws a frame (defined in the window's class description) around the window.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_FRAMECHANGED (0x0020)</term>
        ///         <description>
        ///         Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message
        ///         to the window, even if the window's size is not being changed. If this flag is not specified,
        ///         WM_NCCALCSIZE is sent only when the window's size is being changed
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_HIDEWINDOW (0x0080)</term><description>Hides the window.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOACTIVATE (0x0010)</term>
        ///         <description>
        ///         Does not activate the window. If this flag is not set, the window is activated and moved to
        ///         the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
        ///         parameter).
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOCOPYBITS (0x0100)</term>
        ///         <description>
        ///         Discards the entire contents of the client area. If this flag is not specified, the valid
        ///         contents of the client area are saved and copied back into the client area after the window is sized or
        ///         repositioned.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOMOVE (0x0002)</term>
        ///         <description>Retains the current position (ignores X and Y parameters).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOOWNERZORDER (0x0200)</term>
        ///         <description>Does not change the owner window's position in the Z order.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOREDRAW (0x0008)</term>
        ///         <description>
        ///         Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies
        ///         to the client area, the nonclient area (including the title bar and scroll bars), and any part of the
        ///         parent window uncovered as a result of the window being moved. When this flag is set, the application
        ///         must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOREPOSITION (0x0200)</term><description>Same as the SWP_NOOWNERZORDER flag.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOSENDCHANGING (0x0400)</term>
        ///         <description>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOSIZE (0x0001)</term>
        ///         <description>Retains the current size (ignores the cx and cy parameters).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_NOZORDER (0x0004)</term>
        ///         <description>Retains the current Z order (ignores the hWndInsertAfter parameter).</description>
        ///     </item>
        ///     <item>
        ///         <term>SWP_SHOWWINDOW (0x0040)</term><description>Displays the window.</description>
        ///     </item>
        ///     </list>
        /// </param>
        /// <returns><c>true</c> or nonzero if the function succeeds, <c>false</c> or zero otherwise or if function fails.</returns>
        /// <remarks>
        ///     <para>
        ///     As part of the Vista re-architecture, all services were moved off the interactive desktop into Session 0.
        ///     hwnd and window manager operations are only effective inside a session and cross-session attempts to manipulate
        ///     the hwnd will fail. For more information, see The Windows Vista Developer Story: Application Compatibility
        ///     Cookbook.
        ///     </para>
        ///     <para>
        ///     If you have changed certain window data using SetWindowLong, you must call SetWindowPos for the changes to
        ///     take effect. Use the following combination for uFlags: SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
        ///     SWP_FRAMECHANGED.
        ///     </para>
        ///     <para>
        ///     A window can be made a topmost window either by setting the hWndInsertAfter parameter to HWND_TOPMOST and
        ///     ensuring that the SWP_NOZORDER flag is not set, or by setting a window's position in the Z order so that it is
        ///     above any existing topmost windows. When a non-topmost window is made topmost, its owned windows are also made
        ///     topmost. Its owners, however, are not changed.
        ///     </para>
        ///     <para>
        ///     If neither the SWP_NOACTIVATE nor SWP_NOZORDER flag is specified (that is, when the application requests that
        ///     a window be simultaneously activated and its position in the Z order changed), the value specified in
        ///     hWndInsertAfter is used only in the following circumstances.
        ///     </para>
        ///     <list type="bullet">
        ///     <item>Neither the HWND_TOPMOST nor HWND_NOTOPMOST flag is specified in hWndInsertAfter. </item>
        ///     <item>The window identified by hWnd is not the active window. </item>
        ///     </list>
        ///     <para>
        ///     An application cannot activate an inactive window without also bringing it to the top of the Z order.
        ///     Applications can change an activated window's position in the Z order without restrictions, or it can activate
        ///     a window and then move it to the top of the topmost or non-topmost windows.
        ///     </para>
        ///     <para>
        ///     If a topmost window is repositioned to the bottom (HWND_BOTTOM) of the Z order or after any non-topmost
        ///     window, it is no longer topmost. When a topmost window is made non-topmost, its owners and its owned windows
        ///     are also made non-topmost windows.
        ///     </para>
        ///     <para>
        ///     A non-topmost window can own a topmost window, but the reverse cannot occur. Any window (for example, a
        ///     dialog box) owned by a topmost window is itself made a topmost window, to ensure that all owned windows stay
        ///     above their owner.
        ///     </para>
        ///     <para>
        ///     If an application is not in the foreground, and should be in the foreground, it must call the
        ///     SetForegroundWindow function.
        ///     </para>
        ///     <para>
        ///     To use SetWindowPos to bring a window to the top, the process that owns the window must have
        ///     SetForegroundWindow permission.
        ///     </para>
        /// </remarks>

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// Window handles (HWND) used for hWndInsertAfter
        /// </summary>
        public static class HWND
        {
            public static IntPtr
                NoTopMost = new IntPtr(-2),
                TopMost = new IntPtr(-1),
                Top = new IntPtr(0),
                Bottom = new IntPtr(1);
        }

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
                NOSIZE = 0x0001,
                NOMOVE = 0x0002,
                NOZORDER = 0x0004,
                NOREDRAW = 0x0008,
                NOACTIVATE = 0x0010,
                DRAWFRAME = 0x0020,
                FRAMECHANGED = 0x0020,
                SHOWWINDOW = 0x0040,
                HIDEWINDOW = 0x0080,
                NOCOPYBITS = 0x0100,
                NOOWNERZORDER = 0x0200,
                NOREPOSITION = 0x0200,
                NOSENDCHANGING = 0x0400,
                DEFERERASE = 0x2000,
                ASYNCWINDOWPOS = 0x4000;
        }

        /// <summary>
        ///     Special window handles
        /// </summary>
        public enum SpecialWindowHandles
        {
            // ReSharper disable InconsistentNaming
            /// <summary>
            ///     Places the window at the top of the Z order.
            /// </summary>
            HWND_TOP = 0,
            /// <summary>
            ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
            /// </summary>
            HWND_BOTTOM = 1,
            /// <summary>
            ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
            /// </summary>
            HWND_TOPMOST = -1,
            /// <summary>
            ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
            /// </summary>
            HWND_NOTOPMOST = -2
            // ReSharper restore InconsistentNaming
        }

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            // ReSharper disable InconsistentNaming

            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,

            // ReSharper restore InconsistentNaming
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Find window by Caption, and wait 1/2 a second and then try again.

        public static IntPtr FindWindow(string windowName, bool wait = false)
        {
            var hWnd = FindWindow(null, windowName);
            while (wait && hWnd == IntPtr.Zero)
            {
                Thread.Sleep(500);
                hWnd = FindWindow(null, windowName);
            }

            return hWnd;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public static RECT GetWindowRect(IntPtr hWnd)
        {
            GetWindowRect(hWnd, out var rect);
            return rect;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        public enum SysCommands : int
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,
            //#if(WINVER >= 0x0400) //Win95
            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F,
            //#endif /* WINVER >= 0x0400 */

            //#if(WINVER >= 0x0600) //Vista
            SCF_ISSECURE = 0x00000001,
            //#endif /* WINVER >= 0x0600 */

            /*
              * Obsolete names
              */
            SC_ICON = SC_MINIMIZE,
            SC_ZOOM = SC_MAXIMIZE,
        }

        public const int WM_SYSCOMMAND = 0x112;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
    }

    class Program
    {
        public class ArgData
        {
            public string processName;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public IntPtr? MainWindowHandle;

            public ArgData(string processName, int x, int y, int cx, int cy, IntPtr? mainWindowHandle = null)
            {
                this.x = x;
                this.y = y;
                this.cx = cx;
                this.cy = cy;
                this.processName = processName;
                this.MainWindowHandle = mainWindowHandle;
            }

            public string MainWindowTitle { get; set; }
        }

        static IEnumerable<ArgData> LoadData()
        {
            var args = Properties.Resources.Arg.Split(' ');

            var other = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
                .Where(p => p.Id != Process.GetCurrentProcess().Id)
                .ToArray();

            foreach (var process in other) process.Kill();

            if (other.Any())
                yield break;

            var index = 0;

            while (index < args.Length)
            {
                if (index + 3 > args.Length) break;

                var processName = args[index++];
                var windowPosition = args[index++].Split(';');
                var windowSize = args[index++].Split("xX".ToCharArray());

                if (windowPosition.Length != 2 ||
                    !int.TryParse(windowPosition[0], out var x) ||
                    !int.TryParse(windowPosition[1], out var y))
                    yield break;

                if (windowSize.Length != 2 ||
                    !int.TryParse(windowSize[0], out var cx) ||
                    !int.TryParse(windowSize[1], out var cy))
                    yield break;

                yield return new ArgData(processName, x, y, cx, cy);
            }
        }

        static void Tile(params ArgData[] argData)
        {
            while (true)
            {
                foreach (var d in argData)
                {
                    var proc = GetProcess(d.processName);
                    if (proc == null) continue;

                    d.MainWindowHandle = proc.MainWindowHandle;
                    d.MainWindowTitle = proc.MainWindowTitle;
                }

                if (argData.Any(d => d.MainWindowHandle != null))
                {
                    var taskbar = Api.FindWindow("Shell_TrayWnd", null);
                    var taskbarRect = Api.GetWindowRect(taskbar);

                    var taskbarX = taskbarRect.Left;
                    var taskbarY = taskbarRect.Top;
                    var taskbarCX = taskbarRect.Right - taskbarRect.Left;
                    var taskbarCY = taskbarRect.Bottom - taskbarRect.Top;

                    Api.SetWindowPos(taskbar, (IntPtr)Api.SpecialWindowHandles.HWND_NOTOPMOST, taskbarX, taskbarY, taskbarCX,
                        taskbarCY, Api.SetWindowPosFlags.SWP_SHOWWINDOW);
                }

                foreach (var d in argData.Where(d => d.MainWindowHandle != null))
                {
                    if (d.processName == "Zona" && d.MainWindowTitle == "Zona" || string.IsNullOrEmpty(d.MainWindowTitle))
                    {
                        if (string.IsNullOrEmpty(d.MainWindowTitle) &&
                            d.MainWindowHandle.HasValue && Api.GetWindowRect(d.MainWindowHandle.Value, out var rect))
                        {
                            //3668;658 162x372
                            if (rect.Right - rect.Left == 162 &&
                                rect.Bottom - rect.Top == 372)
                            {
                                Log.Debug("[{MainWindowTitle}] {ProcessName} {Rect} SendMessage SC_CLOSE",
                                    d.MainWindowTitle,
                                    d.processName,
                                    $"{{{rect.Left};{rect.Top} {rect.Right - rect.Left}x{rect.Bottom - rect.Top}}}");
                                Api.SendMessage(d.MainWindowHandle.Value, Api.WM_SYSCOMMAND, (int)Api.SysCommands.SC_CLOSE, IntPtr.Zero);
                            }
                        }

                        continue;
                    }

                    // if (!d.MainWindowHandle.HasValue ||
                    //     !Api.GetWindowRect(d.MainWindowHandle.Value, out var r) ||
                    //     r.Left == d.x && r.Top == d.y && r.Right - r.Left == d.cx && r.Bottom - r.Top == d.cy) continue;
                    //
                    // Log.Debug("[{MainWindowTitle}] {ProcessName} {Rect} SetWindowPos",
                    //     d.MainWindowTitle,
                    //     d.processName);

                    if (!d.MainWindowHandle.HasValue) continue;

                    Api.SetWindowPos(d.MainWindowHandle.Value, (IntPtr)Api.SpecialWindowHandles.HWND_TOP, d.x, d.y, d.cx, d.cy,
                        Api.SetWindowPosFlags.SWP_SHOWWINDOW);
                }

                if (argData.FirstOrDefault(d => d.processName == "Zona") == null)
                {
                    var zonaProc = GetProcess("Zona");
                    if (zonaProc != null && !string.IsNullOrEmpty(zonaProc.MainWindowTitle) && zonaProc.MainWindowTitle != "Zona")
                    {
                        var r = Api.GetWindowRect(zonaProc.MainWindowHandle);
                        argData = argData.Append(new ArgData("Zona", r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top, zonaProc.MainWindowHandle)).ToArray();
                    }
                }

                Thread.Sleep(500);
            }
        }

        static void Diagnostic()
        {
            while (true)
            {
                foreach (var process in Process.GetProcesses())
                {
                    if (process.ProcessName != "Zona") continue;

                    using (LogContext.PushProperty("ProcessName", process.ProcessName))
                    using (LogContext.PushProperty("MainWindowTitle", process.MainWindowTitle))
                    {
                        var hwnd = process.MainWindowHandle;
                        if (hwnd != IntPtr.Zero)
                        {
                            if (Api.GetWindowRect(hwnd, out var rect))
                            {
                                Log.Debug("[{MainWindowTitle}] {ProcessName} {Rect}",
                                    process.MainWindowTitle,
                                    process.ProcessName,
                                    $"{{{rect.Left};{rect.Top} {rect.Right - rect.Left}x{rect.Bottom - rect.Top}}}");
                            }
                            // else
                            //     Log.Information("GetWindowRect return false");
                        }
                        // else
                        //     Log.Information("MainWindowHandle is IntPtr.Zero");
                    }
                }

                Console.ReadKey();
                Thread.Sleep(500);
                Console.Clear();
            }
        }

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            try
            {
                var watcher = new InstanceWatcher("windowstilerpipeserver");
                watcher.RunUnRegisterInstance += (sender, s) => Environment.Exit(0);
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }

            var data = new List<window>();
            data.Add(new window()
            {
                process = "ShooterGame",
                mode = windowMode.hold,
                position = new List<windowPosition>()
                {
                    new windowPosition(){X=0, Y=0},
                    new windowPosition(){X=960, Y=0},
                    new windowPosition(){X=1920, Y=0}
                },
                size = new List<windowSize>()
                {
                    new windowSize(){width=1920, height=1080}
                }
            });
            data.Add(new window()
            {
                process = "Zona",
                mode = windowMode.close,
                condition = new List<windowCondition>()
                {
                    new windowCondition()
                    {
                        title=new List<windowConditionTitle>()
                        {
                            new windowConditionTitle(){value="Zona", mode=windowConditionTitleMode.equals },
                            new windowConditionTitle(){isempty=true}
                        }
                    },
                    new windowCondition()
                    {
                        width = new windowConditionWidth(){value=162, accuracy=10},
                        height = new windowConditionHeight(){value=372, accuracy=10}
                    }
                }
            });
            data.Add(new window()
            {
                process = "Zona",
                mode = windowMode.remember,
                condition = new List<windowCondition>()
                {
                    new windowCondition()
                    {
                        title=new List<windowConditionTitle>()
                        {
                            new windowConditionTitle(){value="Zona", mode=windowConditionTitleMode.notequals}
                        }
                    },
                    new windowCondition()
                    {
                        title=new List<windowConditionTitle>()
                        {
                            new windowConditionTitle(){ isempty=false}
                        }
                    }
                }
            });
            data.Add(new window()
            {
                process = "Shell_TrayWnd",
                mode = windowMode.notopmost
            });

            var serializer = new XmlSerializer(typeof(window));
            using var fs = new FileStream("data.xml", FileMode.Create);
            serializer.Serialize(fs, data);

            //Tile(LoadData().ToArray());

            //Diagnostic();
        }

        static Process GetProcess(string processName) => Process.GetProcessesByName(processName).FirstOrDefault();
    }

    public class InstanceWatcher
    {
        private readonly string _pipeServerName;

        public InstanceWatcher(string pipeServerName, string commandToRemoteInstance = "")
        {
            _pipeServerName = pipeServerName;
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;

            if (!IsSinglApplication(commandToRemoteInstance))
                throw new Exception("Is application is copied");

            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                using var server = new NamedPipeServerStream(_pipeServerName);

                server.WaitForConnection();

                using var reader = new StreamReader(server);

                OnRunUnRegisterInstance(reader.ReadToEnd());
            }
        }

        private bool IsSinglApplication(string commandToRemoteInstance)
        {
            try
            {
                using (var client = new NamedPipeClientStream(_pipeServerName))
                {
                    client.Connect(100);

                    if (!string.IsNullOrEmpty(commandToRemoteInstance))
                    {
                        var data = Encoding.UTF8.GetBytes(commandToRemoteInstance);
                        client.Write(data, 0, data.Length);
                    }
                }
            }
            catch (TimeoutException)
            {
                return true;
            }

            return false;
        }

        public event EventHandler<string> RunUnRegisterInstance;

        protected virtual void OnRunUnRegisterInstance(string commandFromRemoteInstance) => RunUnRegisterInstance?.Invoke(this, commandFromRemoteInstance);
    }


}
