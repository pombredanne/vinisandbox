using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class stringMap : EntityTypeConfiguration<string>
    {
        public stringMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.string1)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("string", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.string1).HasColumnName("string");
        }
    }
}
