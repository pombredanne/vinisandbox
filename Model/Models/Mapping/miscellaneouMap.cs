using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class miscellaneouMap : EntityTypeConfiguration<miscellaneou>
    {
        public miscellaneouMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.type)
                .HasMaxLength(65532);

            this.Property(t => t.description)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("miscellaneous", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.miscellaneous)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
