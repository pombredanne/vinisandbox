using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class file
    {
        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        [DisplayName("Fonte")]
        public string source { get; set; }
        [DisplayName("Data de Recebimento")]
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> id_file_detail { get; set; }
        public Nullable<int> id_user { get; set; }
        //public Nullable<bool> analyzed { get; set; }
        public virtual file_detail file_detail { get; set; }
        public virtual user user { get; set; }
    }
}
