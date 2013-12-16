using System;
using System.Collections.Generic;

namespace ViniSandbox.Model.Models
{
    public partial class file_properties
    {
        public file_properties()
        {
            this.antivirus_scan = new List<antivirus_scan>();
            this.comments = new List<comment>();
            this.dns = new List<dn>();
            this.events = new List<@event>();
            this.export_function = new List<export_function>();
            this.files = new List<file>();
            this.miscellaneous = new List<miscellaneou>();
            this.resources = new List<resource>();
            this.result_file = new List<result_file>();
            this.sections = new List<section>();
        }

        public int id { get; set; }
        public byte[] file { get; set; }
        public string file_type { get; set; }
        public string architecture { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public string ssdeep { get; set; }
        public Nullable<System.DateTime> build_date { get; set; }
        public string language { get; set; }
        public string packer { get; set; }
        public Nullable<int> entry_point { get; set; }
        public Nullable<bool> malicious { get; set; }
        public string status { get; set; }
        public virtual ICollection<antivirus_scan> antivirus_scan { get; set; }
        public virtual ICollection<comment> comments { get; set; }
        public virtual ICollection<dn> dns { get; set; }
        public virtual ICollection<@event> events { get; set; }
        public virtual ICollection<export_function> export_function { get; set; }
        public virtual ICollection<file> files { get; set; }
        public virtual ICollection<miscellaneou> miscellaneous { get; set; }
        public virtual ICollection<resource> resources { get; set; }
        public virtual ICollection<result_file> result_file { get; set; }
        public virtual ICollection<section> sections { get; set; }
    }
}
