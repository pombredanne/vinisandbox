using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class antivirus_scan
    {
        public int id { get; set; }
        public string result { get; set; }
        public string av_version { get; set; }
        public Nullable<System.DateTime> av_last_update { get; set; }
        public Nullable<int> id_antivirus { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual antiviru antiviru { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
