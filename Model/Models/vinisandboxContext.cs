using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ViniSandbox.Model.Models.Mapping;

namespace ViniSandbox.Model.Models
{
    public partial class vinisandboxContext : DbContext
    {
        /*static vinisandboxContext()
        {
            Database.SetInitializer<vinisandboxContext>(null);
        }

        public vinisandboxContext()
            : base("Name=vinisandboxContext")
        {
        }

        public DbSet<antiviru> antivirus { get; set; }
        public DbSet<antivirus_scan> antivirus_scan { get; set; }
        public DbSet<comment> comments { get; set; }
        public DbSet<dn> dns { get; set; }
        public DbSet<event> events { get; set; }
        public DbSet<export_function> export_function { get; set; }
        public DbSet<file> files { get; set; }
        public DbSet<file_properties> file_properties { get; set; }
        public DbSet<import_function> import_function { get; set; }
        public DbSet<import_library> import_library { get; set; }
        public DbSet<miscellaneou> miscellaneous { get; set; }
        public DbSet<resource> resources { get; set; }
        public DbSet<result_file> result_file { get; set; }
        public DbSet<section> sections { get; set; }
        public DbSet<string> strings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new antiviruMap());
            modelBuilder.Configurations.Add(new antivirus_scanMap());
            modelBuilder.Configurations.Add(new commentMap());
            modelBuilder.Configurations.Add(new dnMap());
            modelBuilder.Configurations.Add(new eventMap());
            modelBuilder.Configurations.Add(new export_functionMap());
            modelBuilder.Configurations.Add(new fileMap());
            modelBuilder.Configurations.Add(new file_propertiesMap());
            modelBuilder.Configurations.Add(new import_functionMap());
            modelBuilder.Configurations.Add(new import_libraryMap());
            modelBuilder.Configurations.Add(new miscellaneouMap());
            modelBuilder.Configurations.Add(new resourceMap());
            modelBuilder.Configurations.Add(new result_fileMap());
            modelBuilder.Configurations.Add(new sectionMap());
            modelBuilder.Configurations.Add(new stringMap());*/
        }
    }
}
