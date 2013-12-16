using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class import_libraryMap : EntityTypeConfiguration<import_library>
    {
        public import_libraryMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("import_library", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
