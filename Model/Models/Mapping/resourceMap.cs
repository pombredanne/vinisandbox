using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class resourceMap : EntityTypeConfiguration<resource>
    {
        public resourceMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(100);

            this.Property(t => t.language)
                .HasMaxLength(30);

            this.Property(t => t.type)
                .HasMaxLength(65532);

            // Table & Column Mappings
            this.ToTable("resource", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.size).HasColumnName("size");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.resources)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
