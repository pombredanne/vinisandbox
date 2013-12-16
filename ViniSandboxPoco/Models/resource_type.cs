using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class resource_type
    {
        public resource_type()
        {
            this.resources = new List<resource>();
        }

        public int id { get; set; }
        [DisplayName("Tipo")]
        public string name { get; set; }
        public virtual ICollection<resource> resources { get; set; }

        public override bool Equals(object obj)
        {
            return obj is resource_type && ((resource_type)obj).name == name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
