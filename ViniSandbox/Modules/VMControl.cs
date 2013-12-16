
using System.Threading;
using VBox;
namespace ViniSandbox.Modules
{
    public abstract class VMControl
    {
        protected string vmName, username, password, domain;

        public VMControl(string VMName, string Username, string Password, string Domain)
        {
            vmName = VMName;
            username = Username;
            password = Password;
            domain = Domain;
        }

        public abstract void StartVM(string mode);

        public abstract void SuspendVM();

        public abstract void RevertSnapshotVM(string SnapshotName);

        public abstract void CopyFileFromHost(string HostPath, string VMPath);

        public abstract void CopyFileFromGuest(string VMPath, string HostPath);

        public abstract void StartVMProcess(string BinaryPath, string[] args, bool hidden, int timeout);

        public abstract void StartVMProcess(string BinaryPath, string[] args, bool hidden, int timeout, Mutex mutex);

        public abstract void KillVMProcess(string ProcessName);
    }
}
