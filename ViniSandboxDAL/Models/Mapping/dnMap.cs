using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class dnMap : EntityTypeConfiguration<dns>
    {
        public dnMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.domain)
                .HasMaxLength(120);

            // Table & Column Mappings
            this.ToTable("dns", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.domain).HasColumnName("domain");
        }
    }
}
