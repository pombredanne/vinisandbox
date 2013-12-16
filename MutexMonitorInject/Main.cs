using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumerateMutex;
using EasyHook;
using System.Runtime.InteropServices;

namespace MutexMonitorInject
{
    public class Main : EasyHook.IEntryPoint
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner,
          string lpName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner,
          string lpName);

        MutexMonitorInterface Interface;
        LocalHook CreateMutexHook;
        Stack<string> Queue = new Stack<string>();

        static IntPtr CreateMutex_hooked(IntPtr lpMutexAttributes, bool bInitialOwner,
          string lpName)
        {
            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push(lpName);
                }
            }
            catch
            {
            }
            return CreateMutex(lpMutexAttributes, bInitialOwner, lpName);
        }

        public Main(RemoteHooking.IContext InContext, String inChannelName)
        {
            Interface = RemoteHooking.IpcConnectClient<MutexMonitorInterface>(inChannelName);
        }

        public void Run(RemoteHooking.IContext InContext, String inChannelName)
        {
            try
            {
                CreateMutexHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "CreateMutexW"),
                        new DCreateMutex(CreateMutex_hooked), this);
                CreateMutexHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                Interface.ReportException(new Exception("oi"));//ex);
                return;
            }

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            try
            {
                //ss

                if (Queue.Count > 0)
                {
                    String[] Package = null;

                    lock (Queue)
                    {
                        Package = Queue.ToArray();
                        Queue.Clear();
                    }

                    Interface.OnCreateMutex(Package);
                }
                else
                    Interface.Ping();
            }
            catch (Exception)
            {
            }

        }
    }
}
