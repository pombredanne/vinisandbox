using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class dnMap : EntityTypeConfiguration<dn>
    {
        public dnMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.domain)
                .HasMaxLength(70);

            // Table & Column Mappings
            this.ToTable("dns", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.domain).HasColumnName("domain");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.dns)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
