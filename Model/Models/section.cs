using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class section
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> virtual_address { get; set; }
        public Nullable<int> virtual_size { get; set; }
        public Nullable<int> raw_size { get; set; }
        public string md5 { get; set; }
        public Nullable<bool> suspicious { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
