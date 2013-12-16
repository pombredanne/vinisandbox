using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViniSandbox.Models
{
    public partial class user
    {        
        public int id
        { get; set; }

        [Required]
        [DisplayName("Email")]
        public string email
        { get; set; }
      
        [DisplayName("Nome")]
        public string name
        { get; set; }

        [Required]
        [DisplayName("Senha")]
        public string password
        { get; set; }

        [DisplayName("Administrador")]
        public bool admin
        { get; set; }

        [DisplayName("Apelido")]
        public string nickname
        { get; set; }

        [DisplayName("Data de Registro")]
        public Nullable<DateTime> creation_date
        { get; set; }

        public virtual ICollection<file> files { get; set; }

        public virtual ICollection<comment> comments { get; set; }
    }
}
