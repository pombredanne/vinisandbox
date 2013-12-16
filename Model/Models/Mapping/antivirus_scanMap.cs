using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class antivirus_scanMap : EntityTypeConfiguration<antivirus_scan>
    {
        public antivirus_scanMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.result)
                .HasMaxLength(70);

            this.Property(t => t.av_version)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("antivirus_scan", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.result).HasColumnName("result");
            this.Property(t => t.av_version).HasColumnName("av_version");
            this.Property(t => t.av_last_update).HasColumnName("av_last_update");
            this.Property(t => t.id_antivirus).HasColumnName("id_antivirus");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.antiviru)
                .WithMany(t => t.antivirus_scan)
                .HasForeignKey(d => d.id_antivirus);
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.antivirus_scan)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
