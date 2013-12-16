using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
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

            this.Property(t => t.size)
                .HasMaxLength(20);

            this.Property(t => t.language)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("resource", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.size).HasColumnName("size");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.id_type).HasColumnName("id_type");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.pe_file)
                .WithMany(t => t.resources)
                .HasForeignKey(d => d.id_file_properties);
            this.HasOptional(t => t.resource_type)
                .WithMany(t => t.resources)
                .HasForeignKey(d => d.id_type);

        }
    }
}
