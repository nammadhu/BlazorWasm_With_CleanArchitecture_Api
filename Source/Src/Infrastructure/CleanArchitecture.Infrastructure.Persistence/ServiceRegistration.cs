using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTown.Application.Interfaces;
using MyTown.Application.Interfaces.Repositories;
using MyTown.Domain;
using System.Linq;
using System.Reflection;

namespace CleanArchitecture.Infrastructure.Persistence
    {
    public static class ServiceRegistration
        {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {
            services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterRepositories();

            }
        private static void RegisterRepositories(this IServiceCollection services)
            {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var interfaceType = typeof(IGenericRepository<>);
            var interfaces = Assembly.GetAssembly(interfaceType).GetTypes()
                .Where(p => p.GetInterface(interfaceType.Name) != null);

            var implementations = Assembly.GetAssembly(typeof(GenericRepository<>)).GetTypes();

            foreach (var item in interfaces)
                {
                var implimentation = Assembly.GetAssembly(typeof(GenericRepository<>)).GetTypes()
                    .FirstOrDefault(p => p.GetInterface(item.Name.ToString()) != null);
                services.AddTransient(item, implimentation);
                }
            services.AddTransient<ITownRepository, TownRepository>();
            services.AddTransient<ITownCardTypeMasterDataRepository, TownCardTypeMasterDataRepository>();
            services.AddTransient<ITownCardRepository, TownCardRepository>();

            services.AddScoped<IIDGenerator<Town>>(provider =>
       new IDGenerator<Town>(provider.GetService<ApplicationDbContext>(), "Id"));
            services.AddScoped<IIDGenerator<TownCardType>>(provider =>
       new IDGenerator<TownCardType>(provider.GetService<ApplicationDbContext>(), "Id"));
            }
        }
    }
