using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class dns
    {
        public dns()
        {
            this.analyses = new List<analysis>();
        }

        public int id { get; set; }
        [DisplayName("Dominio")]
        public string domain { get; set; }
        public virtual ICollection<analysis> analyses { get; set; }

        public override bool Equals(object obj)
        {
            return obj is dns && ((dns)obj).domain == domain;
        }

        public override int GetHashCode()
        {
            return domain.GetHashCode();
        }
    }
}
