using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ViniSandbox.Models.Mapping
{
    public class computer_eventMap : EntityTypeConfiguration<computer_event>
    {
        public computer_eventMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.process_name)
                .HasMaxLength(255);

            this.Property(t => t.operation)
                .HasMaxLength(45);

            this.Property(t => t.path)
                .HasMaxLength(255);

            this.Property(t => t.result)
                .HasMaxLength(45);

            this.Property(t => t.detail)
                .HasMaxLength(1500);

            // Table & Column Mappings
            this.ToTable("computer_event", "vinisandbox");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.pid).HasColumnName("pid");
            this.Property(t => t.process_name).HasColumnName("process_name");
            this.Property(t => t.time_of_day).HasColumnName("time_of_day");
            this.Property(t => t.operation).HasColumnName("operation");
            this.Property(t => t.path).HasColumnName("path");
            this.Property(t => t.result).HasColumnName("result");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.id_analysis).HasColumnName("id_analysis");

            // Relationships
            this.HasRequired(t => t.analysis)
                .WithMany(t => t.computer_event)
                .HasForeignKey(d => d.id_analysis);

        }
    }
}
