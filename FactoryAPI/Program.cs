using FactoryAPI.Authorization;
using FactoryAPI.Entities;
using FactoryAPI.Middleware;
using FactoryAPI.Models;
using FactoryAPI.Models.Validators;
using FactoryAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Text;
using System.Text.Json.Serialization;

namespace FactoryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var authenticationSettings = new AuthenticationSettings();

            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            builder.Services.AddSingleton(authenticationSettings);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                options.AddPolicy("CreatedAtleast2Restaurants",
                    builder => builder.AddRequirements(new CreatedMultipleFactoryRequirement(2)));
            });

            //builder.Services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();
            //builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            //builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            builder.Services.AddControllers();


            builder.Services.AddAuthorization();
            builder.Services.AddScoped<FactorySeeder>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers().AddJsonOptions(option =>
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSwaggerGen();
            //configure logging
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog();
            });

            builder.Services.AddScoped<IFactoryService, FactoryService>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            // directive DISABLE_FOR_TESTING is helpfull when you need to run your intergation tests
#if DISABLE_FOR_TESTING
            builder.Services.AddDbContext<FactoryDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("FactoryDbConnection")));
#endif


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
            app.UseAuthorization();

            app.UseHttpsRedirection();



            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FactoryDbContext>();
                if (dbContext.Database.IsRelational())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<FactorySeeder>();
                    seeder.Seed();
                    var pendingMigrations = dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                        dbContext.Database.Migrate();
                }
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            app.Run();
        }
    }
}
