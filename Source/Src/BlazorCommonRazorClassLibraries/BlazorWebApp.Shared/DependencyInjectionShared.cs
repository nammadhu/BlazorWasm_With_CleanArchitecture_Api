using BlazorWebApp.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebApp.Shared
    {
    public static class DependencyInjectionShared
        {
        public static IServiceCollection AddDependencyInjectionShared(this IServiceCollection services)
            {
            services.AddScoped<ConfigurationService>();
            services.AddScoped<CommonService>();
            services.AddScoped<AuthService>(); // Add AuthService
            services.AddScoped<HttpBearerTokenInterceptor>(); // Add the interceptor
            services.AddScoped<HttpAnonymousInterceptor>(); // Add the interceptor

            //services.AddFluxor(options =>
            //{
            //    options.ScanAssemblies(typeof(TokenState).Assembly); // Register feature states and reducers
            //});//adding here wont work ,so had to add on main program.cs otherwise that overrides this

            return services;
            }
        }
    }
