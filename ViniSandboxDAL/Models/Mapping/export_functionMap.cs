using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class export_functionMap : EntityTypeConfiguration<export_function>
    {
        public export_functionMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("export_function", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.pe_file_id).HasColumnName("pe_file_id");

            // Relationships
            this.HasRequired(t => t.pe_file)
                .WithMany(t => t.export_function)
                .HasForeignKey(d => d.pe_file_id);

        }
    }
}
