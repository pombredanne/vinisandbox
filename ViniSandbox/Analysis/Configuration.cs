using System;
using System.Xml.Serialization;

namespace ViniSandbox.Analysis
{
    [Serializable]
    [XmlRoot("ViniSandbox")]
    public class Configuration
    {
        public int CheckFrequence
        { get; set; }

        public string TempFolder
        { get; set; }

        public StaticAnalysis StaticAnalysis
        { get; set; }

        public DynamicAnalysis DynamicAnalysis
        { get; set; }
    }
}
