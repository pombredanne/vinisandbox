using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class antiviru
    {
        public antiviru()
        {
            this.antivirus_scan = new List<antivirus_scan>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public virtual ICollection<antivirus_scan> antivirus_scan { get; set; }
    }
}
