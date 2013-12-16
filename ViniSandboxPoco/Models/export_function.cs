using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class export_function
    {
        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        public int pe_file_id { get; set; }
        public virtual pe_file pe_file { get; set; }
    }
}
