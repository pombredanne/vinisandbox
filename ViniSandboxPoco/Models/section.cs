using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class section
    {
        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        [DisplayName("Endereço Virtual")]
        public string virtual_address { get; set; }
        [DisplayName("Tamanho Virtual")]
        public string virtual_size { get; set; }
        [DisplayName("Tamanho em Disco")]
        public string raw_size { get; set; }
        [DisplayName("MD5")]
        public string md5 { get; set; }
        [DisplayName("Suspeito")]
        public Nullable<bool> suspicious { get; set; }
        public int id_file_properties { get; set; }
        public virtual pe_file pe_file { get; set; }
    }
}
