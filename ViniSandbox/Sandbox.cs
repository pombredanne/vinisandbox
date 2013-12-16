using ViniSandbox.Analysis;
using System.Diagnostics;
using System.Threading;
using ViniSandbox.Modules;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ViniSandbox.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using VBox;
using System.Text.RegularExpressions;

namespace ViniSandbox
{
    class Sandbox : IDisposable
    {
        private VMControl vmcontrol;
        private Configuration config;
        private SandboxDAL dal;
        private ObjectExtracted objExtracted;
        private Mutex vmControlMutex = new Mutex(false, VMCONTRO_MUTEX_NAME);
        private const string VMCONTRO_MUTEX_NAME = "ViniSandbox-VmControl";
        private List<Thread> threads = new List<Thread>();

        public Sandbox(Configuration config)
        {
            this.config = config;
            var v = config.DynamicAnalysis.Virtualization;            
            vmcontrol = ModuleLoader<VMControl>.LoadModule(v.VMControlAssembly, v.VMControlClass, v.VMName, v.Username, v.Password, v.Domain);            
            dal = new SandboxDAL();
            objExtracted = dal.addObject;            
        }

        private string getType(string path)
        {
            ProcessStartInfo si = new ProcessStartInfo("file.exe", "\"" + path + "\"");
            si.RedirectStandardOutput = true;
            si.UseShellExecute = false;
            si.WindowStyle = ProcessWindowStyle.Hidden;

            var proc = Process.Start(si);
            string saida = proc.StandardOutput.ReadToEnd();
            saida = Regex.Match(saida, ".+; (?<tipo>.+)\r").Groups["tipo"].Value;
            return saida;
        }

        public void Analyze(file_detail file_det)
        {
            dal.Clean();
            LogManager.WriteLine("Starting analysis: file_details.id = " + file_det.id, LogManager.EVerboseLevel.Normal);
            try
            {
                analysis ana = new analysis();
                ana.start_date = DateTime.Now;

                DirectoryInfo di = new DirectoryInfo(config.TempFolder);
                string name = file_det.files.ToList().FirstOrDefault().name;
                string path = di.FullName + "\\" + name;
                if (File.Exists(path))
                    File.Delete(path);
                File.WriteAllBytes(path, file_det.data);
                string type = getType(path);
                string ext = "exe";
                var fi = new FileInfo(path);
                if (type.ToLower().Contains("dll"))
                    ext = "dll";
                if (fi.Extension != ext)
                {
                    if (File.Exists(fi.FullName + "." + ext))
                        File.Delete(fi.FullName + "." + ext);
                    fi.MoveTo(fi.FullName + "." + ext);

                }
                
                LogManager.WriteLine("Temp File " + fi.FullName + " created", LogManager.EVerboseLevel.Debug);

                LogManager.WriteLine("Static analysis started", LogManager.EVerboseLevel.Debug);
                StaticAnalysis(fi.FullName, config.StaticAnalysis);
                
                revertVM();
                LogManager.WriteLine("VM Reverted", LogManager.EVerboseLevel.Debug);
                string vmMode = config.DynamicAnalysis.Virtualization.VMMode;
                vmMode = String.IsNullOrEmpty(vmMode) ? "headless" : vmMode;
                vmControlMutex.WaitOne();                
                vmcontrol.StartVM(vmMode);
                vmControlMutex.ReleaseMutex();
                LogManager.WriteLine("VM Started", LogManager.EVerboseLevel.Debug);
                LogManager.WriteLine("Dynamic analysis started", LogManager.EVerboseLevel.Debug);
                DynamicAnalysis(fi.FullName, config.DynamicAnalysis);                                               
                
                
                LogManager.WriteLine("Waiting Steps", LogManager.EVerboseLevel.Debug);  
                foreach (var thread in threads)
                {
                    if(thread.ThreadState == System.Threading.ThreadState.Running)
                        thread.Join();
                }
                
                threads.Clear();
                vmControlMutex.WaitOne();
                vmcontrol.SuspendVM();
                vmControlMutex.ReleaseMutex();
                LogManager.WriteLine("VM Suspended", LogManager.EVerboseLevel.Debug);
                
                ana.file_name = fi.Name;
                ana.final_date = DateTime.Now;
                fi.Delete();
                LogManager.WriteLine("Temp File deleted", LogManager.EVerboseLevel.Debug);
                objExtracted(ana);
            }
            catch (Exception ex)
            {
                LogManager.WriteLine("Error on analysis: file_details.id = " + file_det.id + " - " + ex.ToString(), LogManager.EVerboseLevel.Error);
            }

            dal.Save(file_det);
            LogManager.WriteLine("End of analysis: file_details.id = " + file_det.id, LogManager.EVerboseLevel.Normal);           
        }

        private void revertVM()
        {
            vmControlMutex.WaitOne();
            vmcontrol.RevertSnapshotVM(config.DynamicAnalysis.Virtualization.SnapshotName);
            vmControlMutex.ReleaseMutex();
        }

        private void StaticAnalysisStep(object prms)
        {
            object[] objs = (object[])prms;
            string path = (string)objs[0];
            Step step = (Step)objs[1];
            try
            {                
                LogManager.WriteLine(String.Format("Static Analysis: Step started ({0} {1})", step.Path, step.Args), LogManager.EVerboseLevel.Debug);
                if (step.WaitInit > 0)
                    Thread.Sleep(step.WaitInit);

                ProcessStartInfo psi = new ProcessStartInfo(step.Path, step.Args.Replace("{file}", "\"" + new FileInfo(path).FullName + "\""));
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(psi);

                if (step.WaitExit == 0)
                {
                    proc.WaitForExit();
                }
                else if (step.WaitExit > 0)
                {
                    Thread.Sleep(step.WaitExit);
                    proc.Kill();
                }

                if (step.LogAll)
                {
                    objExtracted(new result_file() { program_name = step.LogAllName, data = File.ReadAllBytes(step.LogPath) });
                }
                if (step.LogHandlerAssembly != "" && step.LogHandlerClass != "")
                {
                    LogHandler logHandler = ModuleLoader<LogHandler>.LoadModule(step.LogHandlerAssembly, step.LogHandlerClass); //ModuleLoader<ILogHandler>.LoadModule(step.LogHandlerAssembly, step.LogHandlerClass);
                    logHandler.ParseLog(step.LogPath, objExtracted);
                }
                LogManager.WriteLine(String.Format("Static Analysis: Step ended ({0} {1})", step.Path, step.Args), LogManager.EVerboseLevel.Debug);
            }
            catch (Exception ex)
            {
                LogManager.WriteLine(String.Format("Static Analysis: Step error ({0} {1}) - {2}", step.Path, step.Args, ex.ToString()), LogManager.EVerboseLevel.Error);
            }            
        }

        private void StaticAnalysis(string path, StaticAnalysis sta)
        {
            foreach (var step in sta.Steps)
            {
                Thread ts = new Thread(new ParameterizedThreadStart(StaticAnalysisStep));
                ts.Start(new object[] { path, step });
                threads.Add(ts);
            }
        }

        private string[] ParseLine(string line)
        {
            var insideQuotes = false;

            var parts = new List<string>();

            var j = 0;

            for (var i = 0; i < line.Length; i++)
            {
                if(line[i] == '"')
                {
                    insideQuotes = !insideQuotes;
                    if(!insideQuotes)
                    {
                        parts.Add(line.Substring(j, i - j));
                        j = i + 1;
                    }
                }
                else if(line[i] == ' ')
                {
                    if (!insideQuotes)
                    {
                        parts.Add(line.Substring(j, i - j));
                        j = i + 1;
                    }
                }
                else if (i == line.Length - 1 && !insideQuotes)
                {
                    parts.Add(line.Substring(j));
                }
            }

            return parts.ToArray();
        }       

        private void DynamicAnalysisStep(object prms)
        {
            Step step = (Step)prms; 
            try
            {
                LogManager.WriteLine(String.Format("Dynamic Analysis: Step started ({0} {1})", step.Path, step.Args), LogManager.EVerboseLevel.Debug);
                if (step.WaitInit > 0)
                    Thread.Sleep(step.WaitInit);

                vmControlMutex.WaitOne();
                vmcontrol.StartVMProcess(step.Path, ParseLine(step.Args), true, step.WaitExit, vmControlMutex);                

                if (!string.IsNullOrEmpty(step.LogPath))
                {
                    string logHostPath = Environment.GetEnvironmentVariable("TEMP") + "\\ViniSandbox\\" + Guid.NewGuid();
                    Thread.Sleep(2000);
                    vmControlMutex.WaitOne();
                    vmcontrol.CopyFileFromGuest(step.LogPath, logHostPath);                    
                    vmControlMutex.ReleaseMutex();

                    if (step.LogAll)
                    {
                        objExtracted(new result_file() { program_name = step.LogAllName, data = File.ReadAllBytes(logHostPath) });
                    }
                    if (step.LogHandlerAssembly != "" && step.LogHandlerClass != "")
                    {
                        LogHandler logHandler = ModuleLoader<LogHandler>.LoadModule(step.LogHandlerAssembly, step.LogHandlerClass); //ModuleLoader<ILogHandler>.LoadModule(step.LogHandlerAssembly, step.LogHandlerClass);
                        logHandler.ParseLog(logHostPath, objExtracted);
                    }
                    File.Delete(logHostPath);
                    LogManager.WriteLine(String.Format("Dynamic Analysis: Step ended ({0} {1})", step.Path, step.Args), LogManager.EVerboseLevel.Debug);
                }                
            }
            catch (Exception ex)
            {
                try
                {
                    vmControlMutex.ReleaseMutex();                    
                }
                catch (Exception)
                {
                }
                LogManager.WriteLine(String.Format("Dynamic Analysis: Step error ({0} {1}) - {2}", step.Path, step.Args, ex.ToString()), LogManager.EVerboseLevel.Error);
                Thread.CurrentThread.Abort();
            }
        }

        private void DynamicAnalysis(string path, DynamicAnalysis dyn)
        {
            try
            {
                FileInfo fi = new FileInfo(path);                
                string vmPath = dyn.Virtualization.TempPath + "\\" + fi.Name;
                vmControlMutex.WaitOne();
                vmcontrol.CopyFileFromHost(path, vmPath);
                vmControlMutex.ReleaseMutex();

                foreach (var step in dyn.PreExecution.Steps)
                {
                    Thread ts = new Thread(new ParameterizedThreadStart(DynamicAnalysisStep));
                    ts.Start(step);
                    threads.Add(ts);
                }

                Thread.Sleep(1200);
                vmControlMutex.WaitOne();
                LogManager.WriteLine(String.Format("File {0} Started", fi.Name), LogManager.EVerboseLevel.Debug);
                if (path.EndsWith(".dll"))
                    vmcontrol.StartVMProcess("rundll32.exe", new string[] { vmPath + ",func" }, false, dyn.Execution.Time, vmControlMutex);
                else
                    vmcontrol.StartVMProcess(vmPath, new string[] { }, false, dyn.Execution.Time, vmControlMutex);
                LogManager.WriteLine(String.Format("File {0} Killed", fi.Name), LogManager.EVerboseLevel.Debug);

                foreach (var step in dyn.PosExecution.Steps)
                {
                    Thread ts = new Thread(new ParameterizedThreadStart(DynamicAnalysisStep));
                    ts.Start(step);
                    threads.Add(ts);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    vmControlMutex.ReleaseMutex();
                }
                catch (Exception)
                {
                }
                LogManager.WriteLine("Error on dynamic analysis: " + ex.Message, LogManager.EVerboseLevel.Error);
            }
        }

        public void Dispose()
        {
            vmControlMutex.WaitOne();
            vmcontrol.SuspendVM();
            vmControlMutex.ReleaseMutex();
        }
    }
}
