using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class dn
    {
        public int id { get; set; }
        public string domain { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
