using CleanArchitecture.Infrastructure.Resources.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedResponse;

namespace CleanArchitecture.Infrastructure.Resources
    {
    public static class ServiceRegistration
        {
        public static void AddResourcesInfrastructure(this IServiceCollection services)
            {
            services.AddSingleton<ITranslator, Translator>();
            }
        }
    }
