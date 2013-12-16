using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Model.Models.Mapping
{
    public class eventMap : EntityTypeConfiguration<event>
    {
        public eventMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.process_name)
                .HasMaxLength(255);

            this.Property(t => t.operation)
                .HasMaxLength(45);

            this.Property(t => t.path)
                .HasMaxLength(45);

            this.Property(t => t.result)
                .HasMaxLength(45);

            this.Property(t => t.detail)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("event", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.pid).HasColumnName("pid");
            this.Property(t => t.process_name).HasColumnName("process_name");
            this.Property(t => t.time_span).HasColumnName("time_span");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.path).HasColumnName("path");
            this.Property(t => t.result).HasColumnName("result");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.id_file_properties).HasColumnName("id_file_properties");

            // Relationships
            this.HasOptional(t => t.file_properties)
                .WithMany(t => t.events)
                .HasForeignKey(d => d.id_file_properties);

        }
    }
}
