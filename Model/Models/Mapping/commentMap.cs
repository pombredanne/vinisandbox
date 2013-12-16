using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class commentMap : EntityTypeConfiguration<comment>
    {
        public commentMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.source)
                .HasMaxLength(65532);

            this.Property(t => t.comment1)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("comment", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.source).HasColumnName("source");
            this.Property(t => t.comment1).HasColumnName("comment");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.comments)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
