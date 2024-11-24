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


        [Fact]
        public async Task GetFactory_ForCurrentFactory_ReturnsFactory()
        {

            // act
            var response = await _client.GetAsync("/api/factory/" + 5);


            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetFactory_ForNotExsistingFactory_ReturnsNotFound()
        {

            // act
            var response = await _client.GetAsync("/api/factory/" + 999);


            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
