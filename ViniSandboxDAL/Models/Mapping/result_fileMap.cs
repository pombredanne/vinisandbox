using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class result_fileMap : EntityTypeConfiguration<result_file>
    {
        public result_fileMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.program_name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("result_file", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.data).HasColumnName("data");
            this.Property(t => t.program_name).HasColumnName("program_name");
            this.Property(t => t.id_analysis).HasColumnName("id_analysis");

            // Relationships
            this.HasRequired(t => t.analysis)
                .WithMany(t => t.result_file)
                .HasForeignKey(d => d.id_analysis);

        }
    }
}
