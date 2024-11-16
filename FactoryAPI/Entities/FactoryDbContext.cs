using Microsoft.EntityFrameworkCore;

namespace FactoryAPI.Entities
{
    public class FactoryDbContext : DbContext
    {
        public FactoryDbContext(DbContextOptions<FactoryDbContext> options) : base(options)
        {

        }

        public DbSet<Factory> Factories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Worker> Workers { get; set; }

        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .Property(u => u.Email)
            //    .IsRequired();

            //modelBuilder.Entity<Role>()
            //    .Property(u => u.Name)
            //    .IsRequired();

            modelBuilder.Entity<Factory>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Worker>()
                .Property(d => d.FirstName)
                .IsRequired();

            modelBuilder.Entity<Worker>()
                .Property(d => d.LastName)
                .IsRequired();

            modelBuilder.Entity<Worker>()
                 .Property(d => d.Salary)
                 .IsRequired()
                 .HasPrecision(8, 2);

            modelBuilder.Entity<Worker>()
                 .Property(d => d.JobSeniority)
                 .IsRequired()
                 .HasAnnotation("Range", (0, 50));


            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);
        }

    }
}
