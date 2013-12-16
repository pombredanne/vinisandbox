using System;
using System.Linq;
using VBox;
using ViniSandbox.Modules;
using System.Threading;

namespace ViniSandbox
{
    public class VBoxControl : VMControl
    {        
        /*public static void Main()
        {
            var a = new VirtualBoxClass();
            var m = a.Machines.Single(p => p.Name == VMName);
            var s = new SessionClass();                                    

            if (m.State != VBox.MachineState.MachineState_Running)            
                m.LaunchVMProcess(s, "gui", "").WaitForCompletion(10000);
            else
                m.LockMachine(s, LockType.LockType_Shared);

            var gs = s.Console.Guest.CreateSession("usuario", "", "", "");
            //gs.DirectoryCreate(@"C:\Users\usuario\Desktop\Teste", 0, new DirectoryCreateFlag[] { DirectoryCreateFlag.DirectoryCreateFlag_Parents });
            Console.WriteLine("oi");
            gs.ProcessCreate("calc.exe", null, null, new ProcessCreateFlag[] { ProcessCreateFlag.ProcessCreateFlag_None }, 10000);
            gs.Close();
            s.UnlockMachine();
            
            Console.WriteLine(m.State);

            s.UnlockMachine();
            //var b = s.Console.Guest.Sessions;           
            //b.ProcessCreate("calc.exe", null, null, null, 10000);
                     
            /* Web service funcionando +/-
            vboxService a = new vboxService();
            var ivb = a.IWebsessionManager_logon("", "");
            var session = a.IWebsessionManager_getSessionObject(ivb);
            string muuid = a.IMachine_getId(a.IVirtualBox_getMachines(ivb)[0]);
            a.IVirtualBox_openSession(ivb, session, muuid);
            a.IVirtualBox_openRemoteSession(ivb, session, muuid, "gui", "");                       
        }*/
        
        private IMachine machine;
        private IVirtualBox vbox;

        #region Properties

        public string VMName
        { 
            get{ return vmName; }
        }
        
        public string Username
        { 
            get{ return username; } 
        }
        
        public string Domain
        {
            get { return domain; }  
        }

        #endregion

        public VBoxControl(string VMName, string Username, string Password, string Domain) : base(VMName, Username, Password, Domain)
        {
            try
            {
                vbox = new VirtualBoxClass();
                machine = vbox.Machines.Single(p => p.Name == VMName);                
            }
            catch (Exception ex)
            {
                
                throw;
            }                        
        }

        private VMObjects Instanciate()
        {
            VMObjects vo = new VMObjects();
            vo.Session = new SessionClass();
            machine.LockMachine(vo.Session, LockType.LockType_Shared);
            vo.Console = vo.Session.Console;
            try
            {
                vo.GSession = vo.Console.Guest.CreateSession(username, password, domain, "ViniSandbox");
            }
            catch (Exception)
            {
            }
            return vo;
        }

        private void releaseObjects(VMObjects vo)
        {
            try
            {
                vo.GSession.Close();
            }
            catch (Exception)
            {
            }
            if(vo.Session.State == SessionState.SessionState_Locked)
                vo.Session.UnlockMachine();           
        }

        private class VMObjects
        {
            public Session Session
            {get;set;}
            public IConsole Console
            {get;set;}
            public IGuestSession GSession
            {get;set;}
        }

        public override void StartVM(string mode)
        {
            Session session = new SessionClass();
            if (machine.State == MachineState.MachineState_PoweredOff || machine.State == MachineState.MachineState_Aborted || machine.State == MachineState.MachineState_Saved)
            {
                var prog = machine.LaunchVMProcess(session, mode, "");
                prog.WaitForCompletion(10000);
                
                while (machine.State != MachineState.MachineState_Running)
                    Thread.Sleep(100);

                if(session.State == SessionState.SessionState_Locked)
                    session.UnlockMachine();
            }
            else if(machine.State != MachineState.MachineState_Running)
                throw new Exception(machine.Name + " cannot be launched, because the current status is not Poweredoff or Aborted");
        }

        public override void SuspendVM()
        {           
            VMObjects vo = Instanciate();
            if (machine.State == MachineState.MachineState_Running)                          
                vo.Console.PowerDown().WaitForCompletion(40000);

            releaseObjects(vo);
        }

        public override void RevertSnapshotVM(string SnapshotName)
        {
            SuspendVM();
            VMObjects vo = Instanciate();
            //vo.Session = new SessionClass();
            //machine.LockMachine(vo.Session, LockType.LockType_Shared);
            //vo.Console = vo.Session.Console;
            var snapshot = machine.FindSnapshot(SnapshotName);
            var prog = vo.Console.RestoreSnapshot(snapshot);
            prog.WaitForCompletion(100000);
            //vo.Session.UnlockMachine();
            releaseObjects(vo);
        }

        public override void CopyFileFromHost(string HostPath, string VMPath)
        {
            VMObjects vo = Instanciate();
            IProgress ip = vo.GSession.CopyTo(HostPath, VMPath, new CopyFileFlag[] { CopyFileFlag.CopyFileFlag_None });             
            ip.WaitForCompletion(-1);            
            releaseObjects(vo);
            if (ip.ErrorInfo != null)
                throw new Exception(ip.ErrorInfo.Text);
        }

        public override void CopyFileFromGuest(string VMPath, string HostPath)
        {
            IProgress ip = null;
            VMObjects vo = Instanciate();            
            ip = vo.GSession.CopyFrom(VMPath, HostPath, new CopyFileFlag[] { CopyFileFlag.CopyFileFlag_None });
            ip.WaitForCompletion(-1);                    
            releaseObjects(vo);
            if (ip.ErrorInfo != null)
                throw new Exception(ip.ErrorInfo.Text);
        }

        public override void StartVMProcess(string BinaryPath, string[] args, bool hidden, int timeout)
        {         
            VMObjects vo = Instanciate();
            IGuestProcess ip = null;            
            ip = vo.GSession.ProcessCreate(BinaryPath, args, new string[] { }, new ProcessCreateFlag[] { hidden ? ProcessCreateFlag.ProcessCreateFlag_Hidden : ProcessCreateFlag.ProcessCreateFlag_None }, (uint)timeout);
                //ip.WaitFor((uint)ProcessWaitForFlag.ProcessWaitForFlag_Terminate, (uint)timeout);                            
            releaseObjects(vo);
        }

        public override void StartVMProcess(string BinaryPath, string[] args, bool hidden, int timeout, Mutex m)
        {
            VMObjects vo = Instanciate();
            IGuestProcess ip = null;            
            ip = vo.GSession.ProcessCreate(BinaryPath, args, new string[] { }, new ProcessCreateFlag[] { hidden ? ProcessCreateFlag.ProcessCreateFlag_Hidden : ProcessCreateFlag.ProcessCreateFlag_None }, (uint)timeout);
            m.ReleaseMutex();
            ip.WaitFor((uint)ProcessWaitForFlag.ProcessWaitForFlag_Terminate, (uint)timeout);                
            releaseObjects(vo);
            /*VMObjects vo = Instanciate();
            var ret = vo.GSession.ProcessCreate(BinaryPath, args, new string[] { }, new ProcessCreateFlag[] { hidden ? ProcessCreateFlag.ProcessCreateFlag_Hidden : ProcessCreateFlag.ProcessCreateFlag_None }, (uint)timeout);
            m.ReleaseMutex();
            if(ret.Status == ProcessStatus.ProcessStatus_Started || ret.Status == ProcessStatus.ProcessStatus_Starting)
                ret.WaitFor((uint)ProcessWaitForFlag.ProcessWaitForFlag_Terminate, (uint)timeout);
            releaseObjects(vo);
            if (ret.Status == ProcessStatus.ProcessStatus_Error)
                throw new Exception("Error on start process " + BinaryPath);*/
        }

        public override void KillVMProcess(string ProcessName)
        {
            VMObjects vo = Instanciate();
            foreach (var proc in vo.GSession.Processes)
            {
                if (proc.Name == ProcessName)
                    proc.Terminate();
            }
            releaseObjects(vo);
        }
    }
}
