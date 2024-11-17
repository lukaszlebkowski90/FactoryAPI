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
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            //modelBuilder.Entity<User>()
            //    .Property(u => u.Email)
            //    .IsRequired();

            //modelBuilder.Entity<Role>()
            //    .Property(u => u.Name)
            //    .IsRequired();




        }

    }
}
