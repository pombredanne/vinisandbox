﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using EasyHook;
using System.Runtime.Remoting;

namespace EnumerateMutex
{
    class Win
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(buffer);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_ATTRIBUTES : IDisposable
        {
            public int Length;
            public IntPtr RootDirectory;
            private IntPtr objectName;
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;

            public OBJECT_ATTRIBUTES(string name, uint attrs)
            {
                Length = 0;
                RootDirectory = IntPtr.Zero;
                objectName = IntPtr.Zero;
                Attributes = attrs;
                SecurityDescriptor = IntPtr.Zero;
                SecurityQualityOfService = IntPtr.Zero;

                Length = Marshal.SizeOf(this);
                ObjectName = new UNICODE_STRING(name);
            }

            public UNICODE_STRING ObjectName
            {
                get
                {
                    return (UNICODE_STRING)Marshal.PtrToStructure(
                     objectName, typeof(UNICODE_STRING));
                }

                set
                {
                    bool fDeleteOld = objectName != IntPtr.Zero;
                    if (!fDeleteOld)
                        objectName = Marshal.AllocHGlobal(Marshal.SizeOf(value));
                    Marshal.StructureToPtr(value, objectName, fDeleteOld);
                }
            }

            public void Dispose()
            {
                if (objectName != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(objectName, typeof(UNICODE_STRING));
                    Marshal.FreeHGlobal(objectName);
                    objectName = IntPtr.Zero;
                }
            }
        }
        /*
         * typedef struct _OBJECT_DIRECTORY_INFORMATION {
            UNICODE_STRING Name;
            UNICODE_STRING TypeName;
            } OBJECT_DIRECTORY_INFORMATION, *POBJECT_DIRECTORY_INFORMATION;
         */

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_DIRECTORY_INFORMATION
        {
            public UNICODE_STRING Name;
            public UNICODE_STRING TypeName;
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        /*Windows Functions*/

        [DllImport("ntdll.dll")]
        public static extern int NtOpenDirectoryObject(
           out Microsoft.Win32.SafeHandles.SafeFileHandle DirectoryHandle,
           uint DesiredAccess,
           ref OBJECT_ATTRIBUTES ObjectAttributes);

        [DllImport("ntdll.dll")]
        public static extern int NtQueryDirectoryObject(
           Microsoft.Win32.SafeHandles.SafeFileHandle DirectoryHandle,
           IntPtr Buffer,
           int Length,
           bool ReturnSingleEntry,
           bool RestartScan,
           ref uint Context,
           out uint ReturnLength);

    }

    public class MutexMonitorInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);
        }

        public void ReportException(Exception InInfo)
        {
            Console.WriteLine("The target process has reported" +
                              " an error:\r\n" + InInfo.ToString());
        }

        public void OnCreateMutex(String[] name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                Console.WriteLine(name[i]);
            }
        }

        public void Ping()
        {
        }
    }

    class Program
    {
        static String ChannelName = null;

        static void Main(string[] args)
        {
            int i = 0;
            foreach (var item in Process.GetProcessesByName("ViniSandbox"))
            {
                i = item.Id;
            }
            try
            {
                Config.Register(
                    "Teste",
                    "MutexMonitor.exe",
                    "MutexMonitorInject.dll");

                RemoteHooking.IpcCreateServer<MutexMonitorInterface>(
                     ref ChannelName, WellKnownObjectMode.SingleCall);

                RemoteHooking.Inject(
                    //Int32.Parse(args[0]),
                    i,
                    "MutexMonitorInject.dll",
                    "MutexMonitorInject.dll",
                    ChannelName);

                Console.ReadLine();
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting " +
                                  "to target:\r\n{0}", ExtInfo.ToString());
            }
           /* Microsoft.Win32.SafeHandles.SafeFileHandle h;
            string dEntry = "\\";
            ArrayList entries = new ArrayList();
            entries.Add(dEntry);
            while (entries.Count != 0)
            {
                foreach (String entry in entries)
                {
                    var attr = new Win.OBJECT_ATTRIBUTES(entry, 0);
                    var st = Win.NtOpenDirectoryObject(out h, 1, ref attr);
                    if (st < 0)
                    {
                        h.Dispose();
                        entries.Remove(entry);
                        break;
                    }

                    var bufsz = 1024;
                    var buf = Marshal.AllocHGlobal(bufsz);
                    uint context = 0, len;
                    while (true)
                    {
                        st = Win.NtQueryDirectoryObject(h, buf, bufsz, true, context == 0, ref context, out len);
                        if (st < 0)
                        {
                            entries.Remove(entry);
                            Marshal.FreeHGlobal(buf);
                            h.Dispose();
                            break;
                        }
                        var odi = (Win.OBJECT_DIRECTORY_INFORMATION)
                          Marshal.PtrToStructure(buf, typeof(Win.OBJECT_DIRECTORY_INFORMATION));
                        if (Convert.ToString(odi.TypeName) == "Mutant")
                        {
                            Console.WriteLine("0x{0:X2}:{1,-25}{2}", context, odi.TypeName, odi.Name);
                        }
                        if (Convert.ToString(odi.TypeName) == "Directory")
                        {
                            if (entry == "\\")
                            {
                                entries.Add(entry + Convert.ToString(odi.Name));
                            }
                            else
                            {
                                entries.Add(entry + "\\" + Convert.ToString(odi.Name));
                            }
                        }
                    }
                    break;
                }
            }*/
        }
    }
}
