using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class import_function
    {
        public import_function()
        {
            this.pe_file = new List<pe_file>();
        }

        public int id { get; set; }
        [DisplayName("Offset")]
        public string offset { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        public int import_library_id { get; set; }
        public virtual import_library import_library { get; set; }
        public virtual ICollection<pe_file> pe_file { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is import_function)
            {
                import_function aux = (import_function)obj;
                return aux.name == name && aux.offset == offset && import_library != null && import_library.Equals(aux.import_library);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (name + "_" + offset + (import_library == null ? "" : ("_" + import_library.name))).GetHashCode();
        }
    }
}
