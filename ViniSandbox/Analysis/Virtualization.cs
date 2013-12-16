using System;

namespace ViniSandbox.Analysis
{
    [Serializable]
    public class Virtualization
    {        
        public string SnapshotName
        { get; set; }
        
        public string VMName
        { get; set; }

        public string VMMode
        { get; set; }

        public string ViniSandboxToolsPath
        { get; set; }

        public string TempPath
        { get; set; }

        public string VMControlAssembly
        { get; set; }

        public string VMControlClass
        { get; set; }

        public string Username
        { get; set; }

        public string Password
        { get; set; }

        public string Domain
        { get; set; }
    }
}
