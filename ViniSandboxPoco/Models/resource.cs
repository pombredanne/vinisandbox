using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class resource
    {
        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        [DisplayName("Tamanho")]
        public string size { get; set; }
        [DisplayName("Idioma")]
        public string language { get; set; }
        public Nullable<int> id_type { get; set; }
        public Nullable<int> id_file_properties { get; set; }
        public virtual pe_file pe_file { get; set; }
        public virtual resource_type resource_type { get; set; }
    }
}
