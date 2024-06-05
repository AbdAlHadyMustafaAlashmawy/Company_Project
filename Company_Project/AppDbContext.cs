using Company_Project.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Company_Project
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config =configuration.GetSection("ConnectionString").Value;
            optionsBuilder.UseSqlServer(config);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>()
            .HasIndex(p => p.Name)
            .IsUnique();
            modelBuilder.Entity<Companies>()
            .HasIndex(p => p.Name)
            .IsUnique();
            modelBuilder.Entity<Departments>()
            .HasIndex(p => p.Name)
            .IsUnique();
            modelBuilder.Entity<Products>()
            .HasOne(p => p.department) 
            .WithMany(d=>d.products) 
            .HasForeignKey(p => p.Department_no)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Departments>()
            .HasOne(x=>x.company)
            .WithMany(x=>x.Departments).
             OnDelete(DeleteBehavior.Cascade).IsRequired();
        }

    }   
}
