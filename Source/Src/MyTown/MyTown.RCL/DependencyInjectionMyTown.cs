using MyTown.RCL.Town;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyTown.RCL
    {
    public static class DependencyInjectionMyTown
        {
        public static IServiceCollection AddDependencyInjectionMyTown(this IServiceCollection services)
            {
            services.AddScoped<TownService>();
            services.AddAutoMapper(config =>
            { config.AddMaps(Assembly.GetExecutingAssembly(), typeof(MyTown.SharedModels.DTOs.TownDto).Assembly); });
            //if above works then delete below & also remove interfaces
            //services.AddScoped<ITownCardTypeService, TownCardTypeService>();
            return services;
            }
        }
    }
