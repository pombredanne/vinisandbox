using System;
using System.Collections.Generic;

namespace ViniSandbox.Models
{
    public partial class result_file
    {
        public int id { get; set; }
        public byte[] data { get; set; }
        public string program_name { get; set; }
        public int id_analysis { get; set; }
        public virtual analysis analysis { get; set; }
    }
}
