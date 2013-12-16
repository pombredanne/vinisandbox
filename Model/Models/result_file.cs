using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class result_file
    {
        public int id { get; set; }
        public byte[] data { get; set; }
        public string program_name { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
