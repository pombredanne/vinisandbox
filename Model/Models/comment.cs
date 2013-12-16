using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class comment
    {
        public int id { get; set; }
        public string source { get; set; }
        public string comment1 { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
