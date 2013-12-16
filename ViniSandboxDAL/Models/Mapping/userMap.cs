using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViniSandbox.Models.Mapping
{
    public class userMap : EntityTypeConfiguration<user>
    {
        public userMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
            this.Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.email)
                .HasMaxLength(45)
                .IsRequired();
            this.Property(t => t.name)
                .HasMaxLength(60);
            this.Property(t => t.password)
                .HasMaxLength(32)
                .IsRequired();
            this.Property(t => t.admin);
            this.Property(t => t.creation_date);
            this.Property(t => t.nickname)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("user", "vinisandbox");
        }
    }
}
