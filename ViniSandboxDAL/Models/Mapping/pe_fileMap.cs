using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class pe_fileMap : EntityTypeConfiguration<pe_file>
    {
        public pe_fileMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.architecture)
                .HasMaxLength(45);

            this.Property(t => t.language)
                .HasMaxLength(45);

            this.Property(t => t.packer)
                .HasMaxLength(70);

            this.Property(t => t.entry_point)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("pe_file", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.architecture).HasColumnName("architecture");
            this.Property(t => t.compilation_date).HasColumnName("compilation_date");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.packer).HasColumnName("packer");
            this.Property(t => t.entry_point).HasColumnName("entry_point");

            // Relationships
            this.HasRequired(t => t.file_detail)
                .WithOptional(t => t.pe_file);

        }
    }
}
