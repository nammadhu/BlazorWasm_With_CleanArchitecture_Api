using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Application
    {
    public static class ServiceRegistration
        {
        public static void AddApplicationLayer(this IServiceCollection services)
            {
            services.AddAutoMapper(config => { config.AddMaps(Assembly.GetExecutingAssembly()); });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));//https://github.com/jbogard/MediatR

            }
        }
    }
