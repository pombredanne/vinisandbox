using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class antivirus_scan
    {
        [DisplayName("Resultado")]
        public string result { get; set; }
        [DisplayName("Versão")]
        public string av_version { get; set; }
        public Nullable<System.DateTime> av_last_update { get; set; }
        public int id_antivirus { get; set; }
        public int id_analysis { get; set; }
        public virtual analysis analysis { get; set; }
        public virtual antivirus antivirus { get; set; }
    }
}
