using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class analysisMap : EntityTypeConfiguration<analysis>
    {
        public analysisMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.file_name)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("analysis", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.start_date).HasColumnName("start_date");
            this.Property(t => t.final_date).HasColumnName("final_date");
            this.Property(t => t.id_file_detail).HasColumnName("id_file_detail");
            this.Property(t => t.file_name).HasColumnName("file_name");

            // Relationships
            this.HasMany(t => t.dns)
                .WithMany(t => t.analyses)
                .Map(m =>
                    {
                        m.ToTable("analysis_dns", "vinisandbox");
                        m.MapLeftKey("id_analysis");
                        m.MapRightKey("id_dns");
                    });

            this.HasRequired(t => t.file_detail)
                .WithMany(t => t.analyses)
                .HasForeignKey(d => d.id_file_detail);

        }
    }
}
