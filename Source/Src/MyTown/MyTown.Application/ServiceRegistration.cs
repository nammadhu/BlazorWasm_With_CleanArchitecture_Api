using CleanArchitecture.Application;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyTown.Application
    {
    public static class ServiceRegistration
        {
        public static void AddMyTownApplicationLayer(this IServiceCollection services)
            {
            services.AddApplicationLayer(); //separately not required because thats also part of this assembly as derived from it
            services.AddAutoMapper(config =>
            { config.AddMaps(Assembly.GetExecutingAssembly(), typeof(TownCardTypeDto).Assembly); });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));//https://github.com/jbogard/MediatR
            }
        }
    }
