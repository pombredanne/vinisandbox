using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Rug.Cmd;

namespace ViniSandboxTools
{
    class Program
    {
        private const UInt32 WM_CLOSE = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        delegate void EnumMainWindowHandle(IntPtr handle);

        static void ProcessMainWindowHandle(string ProcessName, EnumMainWindowHandle enumFunc)
        {
            List<IntPtr> list = new List<IntPtr>();
            Process[] procs = Process.GetProcessesByName(ProcessName);
            foreach (var proc in procs)
	        {
                if (proc.MainWindowHandle == IntPtr.Zero)
                    enumFunc(proc.Parent().MainWindowHandle);
                else
                    enumFunc(proc.MainWindowHandle);
	        }
        }

        static void takeScreenshot(string outputPath)
        {

            using (Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                bmpScreenCapture.Save(outputPath);
            }
        }

        private static void HideWindow(IntPtr Whandle)
        {
            ShowWindow(Whandle, 0);
        }

        private static void HideWindow(string ProcessName)
        {
            ProcessMainWindowHandle(ProcessName, new EnumMainWindowHandle(HideWindow));
        }

        private static void SendCloseSignal(IntPtr handle)
        {
            PostMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        private static void SendCloseSignal(string ProcessName)
        {
            ProcessMainWindowHandle(ProcessName, new EnumMainWindowHandle(SendCloseSignal));
        }
        
        static int Main(string[] args)
        { 
            StringArgument ss = new StringArgument("ss", "Screenshot", "Path to destination image file");
            StringArgument hw = new StringArgument("hw", "Hide Window", "Process name to hide main process window");
            StringArgument cs = new StringArgument("cs", "Send Close Signal", "Process name to send a close signal");
            ArgumentParser parser = new ArgumentParser("ViniSandboxTools", "This application is able to take screenshots, hide main window from a process and send close signal to a process.");
            parser.Add("\\", "ss", ss);
            parser.Add("\\", "hw", hw);
            parser.Add("\\", "cs", cs);

            if (parser.HelpMode)
            {
                parser.WriteLongArgumentsUsage();
                return 0;
            }

            try
            {
                parser.Parse(args);
            }
            catch (Exception ex)
            {
                parser.WriteShortArgumentsUsage();
                return -1;
            }

            try
            {
                if (hw.Defined)
                {
                    HideWindow(hw.Value);
                }                
                if (cs.Defined)
                {
                    SendCloseSignal(cs.Value);
                }
                if (ss.Defined)
                {
                    takeScreenshot(ss.Value);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }            
        }
    }
}
