using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class miscellaneouMap : EntityTypeConfiguration<miscellaneous>
    {
        public miscellaneouMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.type)
                .HasMaxLength(65532);

            this.Property(t => t.description)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("miscellaneous", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.id_analysis).HasColumnName("id_analysis");

            // Relationships
            this.HasRequired(t => t.analysis)
                .WithMany(t => t.miscellaneous)
                .HasForeignKey(d => d.id_analysis);

        }
    }
}
