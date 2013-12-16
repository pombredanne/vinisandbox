using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class pe_file
    {
        public pe_file()
        {
            this.export_function = new List<export_function>();
            this.resources = new List<resource>();
            this.sections = new List<section>();
            this.import_function = new List<import_function>();
        }

        public int id { get; set; }
        [DisplayName("Nome")]
        public string architecture { get; set; }
        [DisplayName("Data de Compilação")]
        public Nullable<System.DateTime> compilation_date { get; set; }
        [DisplayName("Idioma")]
        public string language { get; set; }
        [DisplayName("Packer")]
        public string packer { get; set; }
        [DisplayName("Entry Point")]
        public string entry_point { get; set; }
        public virtual ICollection<export_function> export_function { get; set; }
        public virtual file_detail file_detail { get; set; }
        public virtual ICollection<resource> resources { get; set; }
        public virtual ICollection<section> sections { get; set; }
        public virtual ICollection<import_function> import_function { get; set; }
    }
}
