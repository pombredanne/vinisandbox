using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class miscellaneou
    {
        public int id { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
