using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class file_propertiesMap : EntityTypeConfiguration<file_properties>
    {
        public file_propertiesMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.file_type)
                .HasMaxLength(45);

            this.Property(t => t.architecture)
                .HasMaxLength(20);

            this.Property(t => t.md5)
                .HasMaxLength(32);

            this.Property(t => t.sha1)
                .HasMaxLength(44);

            this.Property(t => t.ssdeep)
                .HasMaxLength(150);

            this.Property(t => t.language)
                .HasMaxLength(30);

            this.Property(t => t.packer)
                .HasMaxLength(70);

            this.Property(t => t.status)
                .HasMaxLength(65532);

            // Table & Column Mappings
            this.ToTable("file_properties", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.file).HasColumnName("file");
            this.Property(t => t.file_type).HasColumnName("file_type");
            this.Property(t => t.architecture).HasColumnName("architecture");
            this.Property(t => t.md5).HasColumnName("md5");
            this.Property(t => t.sha1).HasColumnName("sha1");
            this.Property(t => t.ssdeep).HasColumnName("ssdeep");
            this.Property(t => t.build_date).HasColumnName("build_date");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.packer).HasColumnName("packer");
            this.Property(t => t.entry_point).HasColumnName("entry_point");
            this.Property(t => t.malicious).HasColumnName("malicious");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}
