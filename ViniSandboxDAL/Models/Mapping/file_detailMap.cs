using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class file_detailMap : EntityTypeConfiguration<file_detail>
    {
        public file_detailMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.type)
                .HasMaxLength(120);

            this.Property(t => t.md5)
                .HasMaxLength(32);

            this.Property(t => t.sha1)
                .HasMaxLength(40);

            this.Property(t => t.sha256)
                .HasMaxLength(64);

            this.Property(t => t.sha512)
                .HasMaxLength(128);

            this.Property(t => t.crc32)
                .HasMaxLength(8);

            this.Property(t => t.ssdeep)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("file_detail", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.md5).HasColumnName("md5");
            this.Property(t => t.sha1).HasColumnName("sha1");
            this.Property(t => t.sha256).HasColumnName("sha256");
            this.Property(t => t.sha512).HasColumnName("sha512");
            this.Property(t => t.crc32).HasColumnName("crc32");
            this.Property(t => t.ssdeep).HasColumnName("ssdeep");
            this.Property(t => t.malicious).HasColumnName("malicious");
            this.Property(t => t.create_date).HasColumnName("create_date");
            this.Property(t => t.modified_date).HasColumnName("modified_date");
            this.Property(t => t.data).HasColumnName("data");
        }
    }
}
