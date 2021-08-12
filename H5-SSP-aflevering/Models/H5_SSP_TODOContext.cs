using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace H5_SSP_aflevering.Models
{
    public partial class H5_SSP_TODOContext : DbContext
    {
        public H5_SSP_TODOContext()
        {
        }

        public H5_SSP_TODOContext(DbContextOptions<H5_SSP_TODOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoModel> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("H5_SSP_TODO"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TodoModel>(entity =>
            {
                entity.ToTable("Todo");

                entity.Property(e => e.Note).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UserId).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
