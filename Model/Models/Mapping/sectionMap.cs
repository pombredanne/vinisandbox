using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class sectionMap : EntityTypeConfiguration<section>
    {
        public sectionMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(8);

            this.Property(t => t.md5)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("section", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.virtual_address).HasColumnName("virtual_address");
            this.Property(t => t.virtual_size).HasColumnName("virtual_size");
            this.Property(t => t.raw_size).HasColumnName("raw_size");
            this.Property(t => t.md5).HasColumnName("md5");
            this.Property(t => t.suspicious).HasColumnName("suspicious");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.sections)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
