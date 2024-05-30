using Microsoft.Extensions.DependencyInjection;
using PublicCommon;

namespace BlazorWebApp.Shared.Services
    {
    //public interface IConfigurationService
    //{
    //    Task<Settings> LoadSettings();
    //}

    public class ConfigurationService(IHttpClientFactory httpClientFactory, ClientConfig clientConfig, IServiceProvider serviceProvider)//, Settings? appSettingsForClient) //: IConfigurationService
        {
        //private readonly HttpClient _httpClient;

        //public ApiService(IHttpClientFactory httpClientFactory)
        //    {
        //    _httpClient = httpClientFactory.CreateClient("AppConfigClient"); // Named client for configuration
        //    }
        public async Task<ClientConfig> LoadSettings()
            {
            var client = httpClientFactory.CreateClient(CONSTANTS.ClientAnonymous);
            if (clientConfig.Settings is null)
                {
                //check in localstorage,if exists & recent valid then  use it
                using (var scope = serviceProvider.CreateScope())
                    {
                    var localStorage = scope.ServiceProvider.GetRequiredService<Blazored.LocalStorage.ILocalStorageService>();
                    Console.WriteLine("fetching existing local configuration");
                ReadTokenFromLocalOrFetchFresh:
                    var existing = await localStorage.GetOrFetchAndSet<AppSettingsForClient>(ApiEndPoints.ConfigExtractionGet, client);
                    if (existing != null)
                        {
                        if (existing != null && DateTime.Now.Subtract(existing!.LoadedDate).TotalDays < 2)//recent valid offline data in other tabs
                            {
                            return clientConfig.SettingsUpdate(existing);
                            }
                        await localStorage.RemoveItemAsync(ApiEndPoints.ConfigExtractionGet);

                        }
                    goto ReadTokenFromLocalOrFetchFresh;

                    }
                }
            else
                {
                return clientConfig;
                }
            }
        }
    }
