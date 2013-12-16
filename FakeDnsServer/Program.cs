using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Management;
using ARSoft.Tools.Net.Dns;
using System.Runtime.InteropServices;
using Rug.Cmd;
using System.Threading;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace FakeDnsServer
{        
    class Program
    {

        
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);
        private static ManualResetEvent exitEvent = new ManualResetEvent(false);               

        private static bool Handler(CtrlType sig)
        {
                        
            exitEvent.Set();
            return true;
        }               

        private static StringArgument IpArg = new StringArgument("ip", "IP Address", "IP Address to be answered");
        private static StringArgument logFile = new StringArgument("log", "Log File", "Path to log file");

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            ArgumentParser parser = new ArgumentParser("FakeDnsServer", "This application simulate a DNS Server, log all DNS queries and answer always to the specified ip address");
            parser.Add("\\", "ip", IpArg);
            parser.Add("\\", "log", logFile);

            frmMain frm = null;

            try
            {
                parser.Parse(args);
                frm = new frmMain(IpArg.Value, logFile.Value);
            }
            catch (Exception)
            {
                parser.WriteShortArgumentsUsage();
                frm = new frmMain();
            }            
            Application.Run(frm);   
        }
    }
}
