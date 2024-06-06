using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.FileManager;
using CleanArchitecture.Infrastructure.FileManager.Contexts;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Identity.Contexts;
//using CleanArchitecture.Infrastructure.Identity.Models;
using CleanArchitecture.Infrastructure.Identity.Seeds;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Infrastructure.Persistence.Seeds;
using CleanArchitecture.Infrastructure.Resources;
using CleanArchitecture.WebApi.Infrastracture.Extensions;
using CleanArchitecture.WebApi.Infrastracture.Middlewares;
using CleanArchitecture.WebApi.Infrastracture.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MyTown.Application;
using PublicCommon;
using Serilog;
using System.Threading.RateLimiting;

namespace CleanArchitecture.WebApi;
public class Program
    {
    public static async Task Main(string[] args)
        {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        await ConfigureMiddlewareOrder(app);

        app.Run();

        void ConfigureServices(WebApplicationBuilder builder)
            {
            builder.Services.AddMyTownApplicationLayer();

            //for AppConfigurations handling across the app
            builder.Services.AddSingleton<AppConfigurations>();
            AppConfigurations config = new();
            config.Initialize(builder.Configuration);
            builder.Services.AddSingleton(config);
            // Pass the built configuration
            // var configuration = ConfigurationProvider.Configurations;
            bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

            builder.Services.AddPersistenceInfrastructure(builder.Configuration, useInMemoryDatabase);
            builder.Services.AddFileManagerInfrastructure(builder.Configuration, useInMemoryDatabase);
            builder.Services.AddIdentityInfrastructure(builder.Configuration, useInMemoryDatabase);
            builder.Services.AddResourcesInfrastructure();
            builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddSwaggerWithVersioning();
            builder.Services.AddCors(x =>
            {
                x.AddPolicy("Any", b =>
                {
                    b.AllowAnyOrigin();//only for testing
                    b.AllowAnyHeader();
                    b.AllowAnyMethod();
                });
            });
            builder.Services.AddCustomLocalization(builder.Configuration);
            builder.Services.AddHealthChecks();
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

            //var myOptions = new MyRateLimitOptions();
            //builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);
            //var slidingPolicy = "sliding";
            //above can be used to fetch config from appsettings.json
            builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 4;
                    options.Window = TimeSpan.FromSeconds(12);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                }));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
            builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
            }

        async Task ConfigureMiddlewareOrder(WebApplication app)
            {
            using (var scope = app.Services.CreateScope())
                {
                var services = scope.ServiceProvider;

                await services.GetRequiredService<IdentityContext>().Database.MigrateAsync();
                await services.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
                await services.GetRequiredService<FileManagerDbContext>().Database.MigrateAsync();

                //Seed Data
                await DefaultRoles.SeedAsync(services.GetRequiredService<RoleManager<ApplicationRole>>());
                await DefaultBasicUser.SeedAsync(services.GetRequiredService<UserManager<ApplicationUser>>());
                await DefaultData.SeedAsync(services.GetRequiredService<ApplicationDbContext>());
                }
            //TODO MustForProduction
            //if (app.Environment.IsDevelopment())
            //    {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();// c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.WebApi v1"));
                               //}

            app.UseCustomLocalization();
            app.UseCors("Any");
            //app.UseEncryptionValidation();//This works good for client & server but not for aspnet core hosted.

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseSwaggerWithVersioning();
            //app.UseMiddleware<ErrorHandlerMiddleware>();
            //above (ErrorHandlerMiddleware or ExceptionHandlingMiddleware) is old way,(below)GlobalExceptionHandler : IExceptionHandler is new way from asapnet core 8
            app.UseExceptionHandler();//this one enough for All GlobalExceptionHandler,BadRequestExceptionHandler,NotFoundExceptionHandler
            app.UseHealthChecks("/health");
            app.MapControllers();
            app.UseSerilogRequestLogging();

            app.UseRateLimiter();
            }
        }

    }
