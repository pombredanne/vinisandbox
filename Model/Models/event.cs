using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class event
    {
        public int id { get; set; }
        public Nullable<int> pid { get; set; }
        public string process_name { get; set; }
        public Nullable<System.DateTime> time_span { get; set; }
        public string operation { get; set; }
        public string path { get; set; }
        public string result { get; set; }
        public string detail { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual file_properties file_properties { get; set; }
    }
}
