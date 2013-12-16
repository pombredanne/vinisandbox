using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
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
            this.Property(t => t.id_file_detail).HasColumnName("id_file_detail");
            this.Property(t => t.id_user).HasColumnName("id_user");

            // Relationships
            this.HasRequired(t => t.file_detail)
                .WithMany(t => t.comments)
                .HasForeignKey(d => d.id_file_detail);
            this.HasRequired(t => t.user)
                .WithMany(t => t.comments)
                .HasForeignKey(d => d.id_user);

        }
    }
}
