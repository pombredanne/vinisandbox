using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class import_function
    {
        public int id { get; set; }
        public Nullable<int> offset { get; set; }
        public string name { get; set; }
        public Nullable<int> import_library_id { get; set; }
        public virtual import_library import_library { get; set; }
    }
}
