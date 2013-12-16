using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
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
            this.Property(t => t.file_properties_id).HasColumnName("file_properties_id");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.export_function)
                .HasForeignKey(d => d.file_properties_id);

        }
    }
}
