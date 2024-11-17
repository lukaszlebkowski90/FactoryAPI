using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryAPI.Entities.Configurations
{
    public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder
                .Property(d => d.FirstName)
                .IsRequired();

            builder
                .Property(d => d.LastName)
                .IsRequired();

            builder
                 .Property(d => d.Salary)
                 .IsRequired()
                 .HasPrecision(8, 2);

            builder
                 .Property(d => d.JobSeniority)
                 .IsRequired()
                 .HasAnnotation("Range", (0, 50));

            builder
                .Property(e => e.FullName)
                .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
        }
    }
}
