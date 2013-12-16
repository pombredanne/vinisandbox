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

        private static void HideWindow()
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

        public static void Main(string[] args)
        {            
            if (IsSingleInstance())
            {
                StringArgument silentArg = new StringArgument("quiet", "Hide application window", "Run application in background");                
                StringArgument logFileArg = new StringArgument("log", "Log File", "Path to log file");
                StringArgument verbArg = new StringArgument("verbose", "Verbose level", "3 - Debug\r\n2 - Normal\r\n1 - Error");
                ArgumentParser argParser = new ArgumentParser("Vinisandbox", "Analyze static and dynamically file");
                argParser.Add("\\", "quiet", silentArg);
                argParser.Add("\\", "log", logFileArg);
                argParser.Add("\\", "verbose", verbArg);

                LogManager.VerboseLevel = LogManager.EVerboseLevel.Normal;

                try
                {
                    if (args.Contains("\\quiet"))
                    {
                        var aux = args.ToList();
                        aux.Remove("\\quiet");
                        args = aux.ToArray();
                        HideWindow();
                    }
                    argParser.Parse(args);                    
                    if(argParser.HelpMode)
                    {
                        argParser.WriteLongArgumentsUsage();
                        Console.Read();
                        return;
                    }                    
                    if (logFileArg.Defined)
                    {
                        LogManager.LogPath = (string)logFileArg.ObjectValue;
                        try
                        {
                            if(!File.Exists(LogManager.LogPath))
                                File.Create(LogManager.LogPath).Close();
                        }
                        catch (Exception)
                        {
                            LogManager.WriteLine("Invalid Log File", LogManager.EVerboseLevel.Error);
                            argParser.WriteShortArgumentsUsage();
                            Console.Read();
                            return;
                        }
                    }
                    if (verbArg.Defined)
                    {
                        try
                        {                           
                            LogManager.VerboseLevel = (LogManager.EVerboseLevel)Enum.ToObject(typeof(LogManager.EVerboseLevel), Convert.ToInt32((string)verbArg.ObjectValue));
                        }
                        catch (Exception)
                        {
                            LogManager.WriteLine("Invalid Verbose level", LogManager.EVerboseLevel.Error);
                            argParser.WriteShortArgumentsUsage();
                            Console.Read();
                            return;
                        }
                    }
                    LogManager.WriteLine("Arguments Parsed", LogManager.EVerboseLevel.Debug);                    
                }
                catch (Exception)
                {
                }

                vinisandboxContext cx = null;
                try
                {
                    cx = new vinisandboxContext();
                    LogManager.WriteLine("DbContext created", LogManager.EVerboseLevel.Debug);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLine("Error on create DbContext: " + ex.ToString(), LogManager.EVerboseLevel.Error);
                    return;
                }
                //cx.Configuration.ProxyCreationEnabled = true; 
                //cx.Configuration.LazyLoadingEnabled = true;                
                Configuration config = null;
                try
                {
                    config = LoadConfiguration();
                    LogManager.WriteLine("Configuration Loaded", LogManager.EVerboseLevel.Debug);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLine("Error on load configurations:" + ex.ToString(), LogManager.EVerboseLevel.Error);                    
                    return;
                }

                Sandbox sandbox = null;
                try
                {
                    sandbox = new Sandbox(config);
                    LogManager.WriteLine("Sandbox instanciated", LogManager.EVerboseLevel.Debug);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLine("Error on instanciate Sandbox: " + ex.ToString(), LogManager.EVerboseLevel.Error);
                    return;
                }

                try
                {
                    DirectoryInfo di = new DirectoryInfo(config.TempFolder);
                    di.Create();
                    LogManager.WriteLine("Temporary directory created", LogManager.EVerboseLevel.Debug);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLine("Error on create temporary directory: " + ex.ToString(), LogManager.EVerboseLevel.Error);
                    return;
                }

                /*string pat = @"C:\Users\Vinicius\Downloads\sed-4.2.1-setup.exe";               
                file_detail fd = new file_detail();
                fd.data = File.ReadAllBytes(pat);
                fd.files = new List<file>();
                fd.files.Add(new file() { name = "sed-4.2.1-setup.exe" });
                cx.file_detail.Add(fd);
                cx.SaveChanges();*/

                LogManager.WriteLine("Waiting for files", LogManager.EVerboseLevel.Normal);
                while (true)
                {
                    foreach (var file_det in cx.file_detail.Where(p => !p.analyzed.HasValue || !p.analyzed.Value).Include("files").ToList())
                    {
                        sandbox.Analyze(file_det);
                        /*foreach (var file in file_det.files)
                        {
                            file.analyzed = true;
                        }*/
                        file_det.analyzed = true;
                        try
                        {
                            cx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    Thread.Sleep(config.CheckFrequence);
                }
            }
            else
            {
                Console.WriteLine("Ja existe outra instancia do programa rodando.");
            }                        
        }

        private static Configuration LoadConfiguration()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            using (var xr = XmlReader.Create("config.xml"))
            {
                return (Configuration)serializer.Deserialize(xr);   
            }            
        }
    }
}
