using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class import_library
    {
        public import_library()
        {
            this.import_function = new List<import_function>();
        }

        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        public virtual ICollection<import_function> import_function { get; set; }

        public override bool Equals(object obj)
        {            
            return obj is import_library && ((import_library)obj).name == name;
        }

        public override int GetHashCode()
        {            
            return name.GetHashCode();
        }
    }
}
