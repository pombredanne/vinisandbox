using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class antivirus
    {
        public antivirus()
        {
            this.antivirus_scan = new List<antivirus_scan>();
        }

        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        [DisplayName("Email")]
        public string email { get; set; }
        public virtual ICollection<antivirus_scan> antivirus_scan { get; set; }

        public override bool Equals(object obj)
        {
            return obj is antivirus && ((antivirus)obj).name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
