using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class export_function
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> file_properties_id { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
