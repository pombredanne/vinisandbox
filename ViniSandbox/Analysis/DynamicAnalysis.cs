using System;

namespace ViniSandbox.Analysis
{
    [Serializable]
    public class DynamicAnalysis
    {
        public Virtualization Virtualization
        { get; set; }

        public PreExecution PreExecution
        { get; set; }

        public Execution Execution
        { get; set; }

        public PosExecution PosExecution
        { get; set; }
    }
}
