using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class antivirus_scanMap : EntityTypeConfiguration<antivirus_scan>
    {
        public antivirus_scanMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id_antivirus, t.id_analysis });

            // Properties
            this.Property(t => t.result)
                .HasMaxLength(70);

            this.Property(t => t.av_version)
                .HasMaxLength(45);

            this.Property(t => t.id_antivirus)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.id_analysis)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("antivirus_scan", "vinisandbox");
            this.Property(t => t.result).HasColumnName("result");
            this.Property(t => t.av_version).HasColumnName("av_version");
            this.Property(t => t.av_last_update).HasColumnName("av_last_update");
            this.Property(t => t.id_antivirus).HasColumnName("id_antivirus");
            this.Property(t => t.id_analysis).HasColumnName("id_analysis");

            // Relationships
            this.HasRequired(t => t.analysis)
                .WithMany(t => t.antivirus_scan)
                .HasForeignKey(d => d.id_analysis);
            this.HasRequired(t => t.antivirus)
                .WithMany(t => t.antivirus_scan)
                .HasForeignKey(d => d.id_antivirus);

        }
    }
}
