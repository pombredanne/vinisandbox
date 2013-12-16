using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class resource_typeMap : EntityTypeConfiguration<resource_type>
    {
        public resource_typeMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("resource_type", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
