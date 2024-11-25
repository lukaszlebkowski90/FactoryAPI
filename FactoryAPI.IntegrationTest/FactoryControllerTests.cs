using FactoryAPI.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace FactoryAPI.IntegrationTest
{
    public class FactoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        public FactoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FactoryDbContext>));

                        services.Remove(dbContextOptions);

                        services
                        .AddDbContext<FactoryDbContext>(options => options.UseInMemoryDatabase("FactoryDb"));

                    });
                });

            _client = _factory.CreateClient();
        }

        private void SeedRestaurant(Factory factory)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<FactoryDbContext>();

            _dbContext.Factories.Add(factory);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetFactory_ForCurrentFactory_ReturnsFactory()
        {
            // arrange

            var factory = new Factory()
            {
                CreatedById = 900,
                Description = "test",
                Name = "test",
                Address = new Address()
                {
                    City = "test",
                    Street = "test",
                    PostalCode = "test"
                },
                Workers = new List<Worker>()
                {
                    new Worker()
                    {
                        FirstName = "Test",
                        LastName = "test",
                        Salary = 23456,
                        JobSeniority = 30,
                        FullName = "test"
                    }
                }

            };

            SeedRestaurant(factory);
            // act
            var response = await _client.GetAsync("/api/factory/" + factory.Id);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetFactory_ForNotExsistingFactory_ReturnsNotFound()
        {
            // arrange

            var factory = new Factory()
            {
                CreatedById = 900,
                Description = "test",
                Name = "test",
                Address = new Address()
                {
                    City = "test",
                    Street = "test",
                    PostalCode = "test"
                },
                Workers = new List<Worker>()
                {
                    new Worker()
                    {
                        FirstName = "Test",
                        LastName = "test",
                        Salary = 23456,
                        JobSeniority = 30,
                        FullName = "test"
                    }
                }

            };
            SeedRestaurant(factory);

            // act
            var response = await _client.GetAsync("/api/factory/" + factory.Id + 2);


            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
