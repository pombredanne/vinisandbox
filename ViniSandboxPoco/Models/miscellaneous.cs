using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class miscellaneous
    {
        public int id { get; set; }
        [DisplayName("Tipo")]
        public string type { get; set; }
        [DisplayName("Descrição")]
        public string description { get; set; }
        public int id_analysis { get; set; }
        public virtual analysis analysis { get; set; }
    }
}
