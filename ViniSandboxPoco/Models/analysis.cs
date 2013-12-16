using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class analysis
    {
        public analysis()
        {
            this.antivirus_scan = new List<antivirus_scan>();
            this.computer_event = new List<computer_event>();
            this.miscellaneous = new List<miscellaneous>();
            this.result_file = new List<result_file>();
            this.dns = new List<dns>();
        }

        public int id { get; set; }
        [DisplayName("Inicio")]
        public Nullable<System.DateTime> start_date { get; set; }
        [DisplayName("Fim")]
        public Nullable<System.DateTime> final_date { get; set; }
        public int id_file_detail { get; set; }
        public string file_name { get; set; }
        public virtual file_detail file_detail { get; set; }
        public virtual ICollection<antivirus_scan> antivirus_scan { get; set; }
        public virtual ICollection<computer_event> computer_event { get; set; }
        public virtual ICollection<miscellaneous> miscellaneous { get; set; }
        public virtual ICollection<result_file> result_file { get; set; }
        public virtual ICollection<dns> dns { get; set; }

        public override bool Equals(object obj)
        {
            return obj is analysis && ((analysis)obj).id == id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
