using FactoryAPI.Entities;
using FactoryAPI.Middleware;
using FactoryAPI.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Text.Json.Serialization;

namespace FactoryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<FactorySeeder>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers().AddJsonOptions(option =>
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSwaggerGen();
            //configure logging
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });

            builder.Services.AddScoped<IFactoryService, FactoryService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            builder.Services.AddDbContext<FactoryDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("FactoryDbConnection")));

            var app = builder.Build();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Factory API");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();




            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<FactorySeeder>();
                seeder.Seed();

                var dbContext = scope.ServiceProvider.GetService<FactoryDbContext>();
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                    dbContext.Database.Migrate();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            app.Run();
        }
    }
}
