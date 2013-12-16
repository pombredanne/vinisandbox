using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class file_detail
    {
        public file_detail()
        {
            this.analyses = new List<analysis>();
            this.comments = new List<comment>();
            this.files = new List<file>();
        }

        public int id { get; set; }
        [DisplayName("Tipo")]
        public string type { get; set; }
        [DisplayName("MD5")]
        public string md5 { get; set; }
        [DisplayName("SHA1")]
        public string sha1 { get; set; }
        [DisplayName("SHA256")]
        public string sha256 { get; set; }
        [DisplayName("SHA512")]
        public string sha512 { get; set; }
        [DisplayName("CRC32")]
        public string crc32 { get; set; }
        [DisplayName("SSDEEP")]
        public string ssdeep { get; set; }
        [DisplayName("Malicioso")]
        public Nullable<bool> malicious { get; set; }
        [DisplayName("Enviado para empresas de antivírus")]
        public Nullable<bool> antivirus_sended { get; set; }
        [DisplayName("Análise Pendente")]
        public Nullable<bool> analyzed { get; set; }
        [DisplayName("Data de Criação")]
        public Nullable<System.DateTime> create_date { get; set; }
        [DisplayName("Data de Modificação")]
        public Nullable<System.DateTime> modified_date { get; set; }
        public byte[] data { get; set; }
        public virtual ICollection<analysis> analyses { get; set; }
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<file> files { get; set; }
        public virtual pe_file pe_file { get; set; }


        public override bool Equals(object obj)
        {
            return obj is file_detail && ((file_detail)obj).md5 == md5;
        }

        public override int GetHashCode()
        {
            return (md5 == null ? "" : md5).GetHashCode();
        }
    }
}
