using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ViniSandbox.Analysis;
using ViniSandbox.Models;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Rug.Cmd;

namespace ViniSandbox
{
    class Program
    {        
        private static Mutex programMutex, listMutex = new Mutex(false, LIST_MUTEX_NAME);
        private const string PROGRAM_MUTEX_NAME = "ViniSandbox", LIST_MUTEX_NAME = "ViniSandbox-FIFO", PIPE_NAME = "ViniSandbox";
        private static bool ServerRunning = true;
        private static List<string> files = new List<string>();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void HideWindow()
        {
            ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
        }

        private static bool IsSingleInstance()
        {
            try
            {
                programMutex = Mutex.OpenExisting(PROGRAM_MUTEX_NAME);
                return false;
            }
            catch (Exception)
            {
                programMutex = new Mutex(true, PROGRAM_MUTEX_NAME);
                return true;
            }
        }

        //private static bool checkFile(string path)
        //{
        //    return File.Exists(path);
        //}

        public static void Main(string[] args)
        {            
            if (IsSingleInstance())
            {
                StringArgument silentArg = new StringArgument("quiet", "Hide application window", "Run application in background");
                StringArgument logFileArg = new StringArgument("log", "Log File", "Path to log file");
                StringArgument verbArg = new StringArgument("verbose", "Verbose level", "3 - Debug\r\n2 - Normal\r\n1 - Error");
                ArgumentParser argParser = new ArgumentParser("Vinisandbox", "Analyze static and dynamically file");
                argParser.Add("\\", "\\quiet", silentArg);
                argParser.Add("\\", "\\log", logFileArg);
                argParser.Add("\\", "\\v", verbArg);

                LogManager.VerboseLevel = LogManager.EVerboseLevel.Normal;

                try
                {
                    argParser.Parse(args);

                    if(silentArg.Defined)
                        HideWindow();
                    if (logFileArg.Defined)
                    {
                        LogManager.LogPath = (string)logFileArg.ObjectValue;
                        try
                        {
                            File.Create(LogManager.LogPath);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid Log File");
                            argParser.WriteShortArgumentsUsage();
                            Console.Read();
                            return;
                        }
                    }
                    if (verbArg.Defined)
                    {
                        try
                        {                           
                            LogManager.VerboseLevel = (LogManager.EVerboseLevel)Enum.ToObject(typeof(LogManager.EVerboseLevel), (string)verbArg.ObjectValue);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid Verbose level");
                            argParser.WriteShortArgumentsUsage();
                            Console.Read();
                            return;
                        }
                    }
                    
                }
                catch (Exception)
                {
                }

                vinisandboxContext cx = new vinisandboxContext();
                cx.Configuration.ProxyCreationEnabled = true; 
                cx.Configuration.LazyLoadingEnabled = true;


                Configuration config = LoadConfiguration();
                Sandbox sandbox = new Sandbox(config);

                DirectoryInfo di = new DirectoryInfo(config.TempFolder);
                di.Create();

                /*string pat = @"C:\Users\Vinicius\Downloads\sed-4.2.1-setup.exe";               
                file_detail fd = new file_detail();
                fd.data = File.ReadAllBytes(pat);
                fd.files = new List<file>();
                fd.files.Add(new file() { name = "sed-4.2.1-setup.exe" });
                cx.file_detail.Add(fd);
                cx.SaveChanges();*/
                while (true)
                {
                    foreach (var file_det in cx.file_detail.Where(p => p.files.Count(j => j.analyzed == false) > 0).Include("files").ToList())
                    {
                        sandbox.Analyze(file_det);
                        foreach (var file in file_det.files)
                        {
                            file.analyzed = true;
                        }
                        try
                        {
                            cx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Console.WriteLine("Ja existe outra instancia do programa rodando.");
            }
            //if (args.Length != 1)
            //{
            //    //Erro
            //    Console.WriteLine("Parametros errados.");
            //    return;
            //}
            //if (!checkFile(args[0]))
            //{
            //    Console.WriteLine(args[0] + " não foi encontrado.");
            //    return;
            //}
            //if (IsSingleInstance())
            //{
            //    StartServer();
            //    Sandbox s = new Sandbox(LoadConfiguration());
            //    listMutex.WaitOne();
            //    files.Add(args[0]);
            //    while (files.Count != 0)
            //    {
            //        listMutex.ReleaseMutex();                    
            //        s.Analyze(files[0]);
            //        files.RemoveAt(0);
            //        //faz analise
            //        listMutex.WaitOne();
            //    }
            //    ServerRunning = false;
            //    programMutex.Close();
            //}
            //else
            //{
            //    SendPath(args[0]);
            //}              
        }

        private static Configuration LoadConfiguration()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            using (var xr = XmlReader.Create("config.xml"))
            {
                return (Configuration)serializer.Deserialize(xr);   
            }            
        }

        //private static void SendPath(string Path)
        //{
        //    using (var client = new NamedPipeClientStream(PIPE_NAME))
        //    {
        //        client.Connect();
        //        StreamWriter writer = new StreamWriter(client);
        //        writer.WriteLine(Path);
        //        writer.Flush();
        //    }
            
        //}

        //private static void StartServer()
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        using (var server = new NamedPipeServerStream(PIPE_NAME, PipeDirection.In, 100))
        //        {
        //            while (ServerRunning)
        //            {                        
        //                server.WaitForConnection();                        
        //                StreamReader reader = new StreamReader(server);
        //                listMutex.WaitOne();
        //                files.Add(reader.ReadLine());
        //                listMutex.ReleaseMutex();
        //                server.Disconnect();                        
        //            }
        //        }
        //    });
        //}
    }
}
