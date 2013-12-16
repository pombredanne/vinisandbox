using System;

namespace ViniSandbox.Analysis
{
    [Serializable]
    public class Step
    {
        public string LogAllName
        { get; set; }

        public string Path
        { get; set; }

        public string Args
        { get; set; }

        public string LogPath
        { get; set; }

        public string LogHandlerAssembly
        { get; set; }

        public string LogHandlerClass
        { get; set; }

        public bool LogAll
        { get; set; }

        public int WaitExit
        { get; set; }

        public int WaitInit
        { get; set; }
    }
}
