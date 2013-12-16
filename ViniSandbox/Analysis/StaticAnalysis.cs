using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ViniSandbox.Analysis
{
    [Serializable]
    public class StaticAnalysis
    {
        [XmlArrayItem("Step", typeof(Step))]
        public List<Step> Steps
        { get; set; }
    }
}
