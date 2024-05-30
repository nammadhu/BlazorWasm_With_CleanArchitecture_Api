using Microsoft.Extensions.DependencyInjection;
using MyTown.RCL.Card;
using MyTown.RCL.CardType;
using MyTown.RCL.Town;
using System.Reflection;

namespace MyTown.RCL
    {
    public static class DependencyInjectionMyTown
        {
        public static IServiceCollection AddDependencyInjectionMyTown(this IServiceCollection services)
            {
            services.AddScoped<TownService>();
            services.AddScoped<TownCardTypeService>();
            services.AddScoped<TownCardService>();

            services.AddAutoMapper(config =>
            { config.AddMaps(Assembly.GetExecutingAssembly(), typeof(MyTown.SharedModels.DTOs.TownCardTypeDto).Assembly); });
            //if above works then delete below & also remove interfaces
            //services.AddScoped<ITownCardTypeService, TownCardTypeService>();
            return services;
            }
        }
    }
