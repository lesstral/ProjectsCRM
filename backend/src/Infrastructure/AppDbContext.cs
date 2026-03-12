using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // employee-project many-to-many
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity(j => j.ToTable("ProjectEmployees"));
            // all employees should have unique email
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            // all projects should have unique name
            modelBuilder.Entity<Project>().HasIndex(p => p.Name).IsUnique();
            // project manager one-to-many
            modelBuilder
                .Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(e => e.ManagedProjects)
                .OnDelete(DeleteBehavior.Restrict);

            //set citext for case-insensitive search

            //project
            modelBuilder.Entity<Project>().Property(p => p.Name).HasColumnType("citext");
            modelBuilder
                .Entity<Project>()
                .Property(p => p.CompanyContractorName)
                .HasColumnType("citext");
            modelBuilder
                .Entity<Project>()
                .Property(p => p.CompanyExecutorName)
                .HasColumnType("citext");
            // employee
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasColumnType("citext");
            modelBuilder.Entity<Employee>().Property(e => e.MiddleName).HasColumnType("citext");
            modelBuilder.Entity<Employee>().Property(e => e.LastName).HasColumnType("citext");

            base.OnModelCreating(modelBuilder);
        }
    }
}
