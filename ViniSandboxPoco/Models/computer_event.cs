using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViniSandbox.Models
{
    public partial class computer_event
    {
        public int id { get; set; }
        public Nullable<int> pid { get; set; }
        public string process_name { get; set; }
        public Nullable<System.TimeSpan> time_of_day { get; set; }
        [DisplayName("Operação")]
        public string operation { get; set; }
        [DisplayName("Caminho")]
        public string path { get; set; }
        [DisplayName("Resultado")]
        public string result { get; set; }
        [DisplayName("Detalhes")]
        public string detail { get; set; }
        public int id_analysis { get; set; }
        public virtual analysis analysis { get; set; }
    }
}
