using Bogus;
using FactoryAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryAPI
{
    public class FactorySeeder
    {
        private readonly FactoryDbContext _dbContext;

        private bool isTest => !_dbContext.Database.IsRelational();

        public FactorySeeder(FactoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Database.IsRelational())
                {
                    var pendingMigrations = _dbContext.Database.GetPendingMigrations();

                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _dbContext.Database.Migrate();
                    }
                }

                if (!_dbContext.Factories.Any())
                {
                    var factories = GetFactories(isTest);
                    _dbContext.Factories.AddRange(factories);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Factory> GetFactories(bool isTest)
        {
            int count = isTest ? 5 : 100;
            var factories = new Faker<Factory>()
                    .RuleFor(n => n.Name, f => f.Company.CompanyName())
                    .RuleFor(d => d.Description, f => f.Company.CatchPhrase())
                    .RuleFor(c => c.ContactEmail, f => f.Person.Email)
                    .RuleFor(c => c.ContactNumber, f => f.Person.Phone)
                    .RuleFor(a => a.Address, f =>
                    new Faker<Address>()
                    .RuleFor(c => c.City, f => f.Address.City())
                    .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                    )
                    .RuleFor(w => w.Workers,
                    f => GetWorkers()
                    )
                    .Generate(count);
            return factories;
        }

        public IEnumerable<Worker> GetWorkers()
        {
            List<Worker> workers = new List<Worker>();
            Random random = new Random();
            int numberOfWorkers = random.Next(2, 15);

            for (int i = 0; i < numberOfWorkers; i++)
            {
                workers.Add(
                    new Faker<Worker>()
                        .RuleFor(f => f.FirstName, f => f.Person.FirstName)
                        .RuleFor(l => l.LastName, f => f.Person.LastName)
                        .RuleFor(f => f.FullName, String.Empty)
                        .RuleFor(s => s.Salary, f => f.Finance.Amount(2200, 5700))
                        .RuleFor(j => j.JobSeniority, f => new Random().Next(0, 50))
                        );
            }

            return workers;
        }

    }
}

