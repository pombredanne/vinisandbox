using System;
using System.Collections.Generic;

namespace ViniSandbox.Models
{
    public partial class comment
    {
        public int id { get; set; }
        public string source { get; set; }
        public string comment1 { get; set; }
        public int id_file_detail { get; set; }
        public virtual file_detail file_detail { get; set; }
        public int id_user { get; set; }
        public virtual user user { get; set; }
    }
}
