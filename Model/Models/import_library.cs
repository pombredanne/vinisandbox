using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class import_library
    {
        public import_library()
        {
            this.import_function = new List<import_function>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<import_function> import_function { get; set; }
    }
}
