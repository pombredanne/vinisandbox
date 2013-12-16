using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class fileMap : EntityTypeConfiguration<file>
    {
        public fileMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(255);

            this.Property(t => t.source)
                .HasMaxLength(65532);

            // Table & Column Mappings
            this.ToTable("file", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.source).HasColumnName("source");
            this.Property(t => t.date).HasColumnName("date");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.files)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
