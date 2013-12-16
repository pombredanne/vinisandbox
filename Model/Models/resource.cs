using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class resource
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> size { get; set; }
        public string language { get; set; }
        public string type { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
