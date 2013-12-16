using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class import_functionMap : EntityTypeConfiguration<import_function>
    {
        public import_functionMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("import_function", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.offset).HasColumnName("offset");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.import_library_id).HasColumnName("import_library_id");

            // Relationships
            this.HasOptional(t => t.import_library)
                .WithMany(t => t.import_function)
                .HasForeignKey(d => d.import_library_id);

        }
    }
}
