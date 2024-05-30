using BlazorWebApp.Shared;
using Microsoft.Extensions.DependencyInjection;
using MyTown.RCL;

namespace CommonRazorComponents
    {
    public static class DependencyInjectionCommon
        {
        public static IServiceCollection AddDependencyInjectionCommon(this IServiceCollection services)
            {
            services.AddDependencyInjectionShared();
            services.AddDependencyInjectionMyTown();
            return services;
            }
        }
    }
