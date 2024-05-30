using Blazored.LocalStorage;
using BlazorWebApp.Shared;
using BlazorWebApp.Shared.Services;
using CommonRazorComponents;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using PublicCommon;
using System.Reflection;

namespace BlazorWasmPwaClient
    {
    public class Program
        {
        public static async Task Main(string[] args)
            {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.HostEnvironment.BaseAddress;
            var apiBaseAddress = builder.Configuration.GetSection("Api").Value;
            //builder.HostEnvironment.BaseAddress for aspnet hosted works fine
            var apiUrl = new Uri(apiBaseAddress);
            //had to lock this //todo
            if (string.IsNullOrEmpty(apiBaseAddress)) throw new Exception("Server Api Issue");

            //builder.Services.AddDependencyInjectionMyTown();
            //builder.Services.AddDependencyInjectionShared();
            //above called by below single call
            builder.Services.AddDependencyInjectionCommon();
            //builder.Services.AddDependencyInjectionPwa();//empty so commented
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp =>
            {
                var client = new HttpClient { BaseAddress = apiUrl };
                client.DefaultRequestHeaders.Add(AnonymousHeader.Key, AnonymousHeader.Value); // Optional: Set header directly (optional)
                return client;
            });
            //above is for direct instance of HttpClient client object

            //below are for cleintfactory like  var anonymousClient = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous); or HttpClientFactory.CreateClient("AuthClient")
            builder.Services.AddHttpClient(PublicCommon.CONSTANTS.ClientAnonymous, client =>
            {
                client.BaseAddress = apiUrl;
            }).AddHttpMessageHandler(sp => sp.GetRequiredService<HttpAnonymousInterceptor>());

            builder.Services.AddHttpClient(PublicCommon.CONSTANTS.ClientAuthorized, client =>
            {
                client.BaseAddress = apiUrl;
            }).AddHttpMessageHandler(sp => sp.GetRequiredService<HttpBearerTokenInterceptor>());


            //for loading configurations first time below code,can be accessed anywhere by clientConfig injecting & reading
            builder.Services.AddSingleton<ClientConfig>();
            builder.Services.AddSingleton<ConfigurationService>();
            var configurationService = builder.Build().Services.GetRequiredService<ConfigurationService>();
            var config = await configurationService.LoadSettings();
            builder.Services.AddSingleton(config);
            //for loading configurations first time above code,can be accessed anywhere by clientConfig injecting & reading

            builder.Services.AddSingleton<AppConfigurations>();

            AddAuthenticationTypesCustom(builder, config.Settings);

            //builder.Services.AddAuthorizationCore();
            //builder.Services.AddCascadingAuthenticationState();
            //builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            builder.Services.AddMudServices();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddAutoMapper(config =>
            { config.AddMaps(Assembly.GetExecutingAssembly(), typeof(MyTown.SharedModels.DTOs.TownDto).Assembly); });
            builder.Services.AddLocalization();
            builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly));   //, typeof(TokenState).Assembly));
            //todo if anymore assembly required then addd that also

            await builder.Build().RunAsync();
            }

        private static void AddAuthenticationTypesCustom(WebAssemblyHostBuilder builder, AppSettingsForClient config)
            {
            if (!config.Authentications.IsEmpty())
                {
                var auth = config.Authentications.FirstOrDefault(x => x.Type == CONSTANTS.Auth.ExternalProviders.Google);
                if (auth != null)
                    {
                    //load below based on server data fetched from api
                    builder.Services.AddOidcAuthentication(options =>
                    {
                        //https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library?view=aspnetcore-8.0&tabs=visual-studio

                        // Configure your authentication provider options here.
                        // For more information, see https://aka.ms/blazor-standalone-auth
                        //builder.Configuration.Bind(CONSTANTS.AuthenticationConfigurations.ExternalProviders.Google, options.ProviderOptions);
                        //fetch from api response not locally

                        options.ProviderOptions.Authority = auth.Authority;
                        options.ProviderOptions.ClientId = auth.ClientId;
                        options.ProviderOptions.PostLogoutRedirectUri = auth.PostLogoutRedirectUri;
                        options.ProviderOptions.RedirectUri = auth.RedirectUri;
                        options.ProviderOptions.ResponseType = auth.ResponseType;

                        if (auth.Claims != null && auth.Claims.Any())
                            {
                            foreach (var item in auth.Claims)
                                {
                                options.ProviderOptions.DefaultScopes.Add("email");
                                }
                            }

                        // Request specific Google claims (optional)
                        if (!options.ProviderOptions.DefaultScopes.Contains("email"))
                            options.ProviderOptions.DefaultScopes.Add("email");//this is must for identity management in backend

                        if (!options.ProviderOptions.DefaultScopes.Contains("openid"))
                            options.ProviderOptions.DefaultScopes.Add("openid");

                        if (!options.ProviderOptions.DefaultScopes.Contains("profile"))
                            options.ProviderOptions.DefaultScopes.Add("profile");

                    });
                    }
                /* Configure Facebook authentication
               builder.Services.AddOidcAuthentication(options =>
               {
                   options.ProviderOptions.Authority = "https://www.facebook.com/";
                   options.ProviderOptions.ClientId = "YOUR_FACEBOOK_APP_ID"; // Replace with your actual Facebook app ID
                   options.ProviderOptions.RedirectUri = "https://localhost:5001/authentication/login-callback";
                   // Other options (scopes, post-logout redirect, etc.)
               });
               */
                }
            }
        }
    }
