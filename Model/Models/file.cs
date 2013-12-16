using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class file
    {
        public int id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
