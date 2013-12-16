using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class antiviruMap : EntityTypeConfiguration<antiviru>
    {
        public antiviruMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(45);

            this.Property(t => t.email)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("antivirus", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.email).HasColumnName("email");
        }
    }
}
