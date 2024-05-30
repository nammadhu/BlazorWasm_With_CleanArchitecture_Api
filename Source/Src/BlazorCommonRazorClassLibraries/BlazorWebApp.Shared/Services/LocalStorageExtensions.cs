using Blazored.LocalStorage;
using PublicCommon;
using SharedResponse;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorWebApp.Shared.Services
    {
    public static class LocalStorageExtensions
        {
        private static readonly TimeSpan MaxDefaultTimespan = TimeSpan.FromHours(12);
        const string ExpirationSuffix = "-expiration";

        static string Expiration(string key)
            {
            if (!key.EndsWith(ExpirationSuffix))
                return key + ExpirationSuffix;
            return key;
            }

        public static async Task<JwtSecurityToken?> GetApiTokenFromLocalStorage(this ILocalStorageService localStorage)
            {
            var jwtTokenInString = await localStorage.Get<string>(ApiEndPoints.ApiIssuedJwt);
            //MyLogger.Log("local storage jwttoken is");
            if (string.IsNullOrEmpty(jwtTokenInString))
                {
                MyLogger.Log("null jwt");
                return null;
                }
            //MyLogger.Log(jwtTokenInString);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtTokenInString);
            //NOTE JWT returns compares with UTC always,so comparison should be same
            if (token.ValidTo > DateTime.UtcNow.AddSeconds(5))
                {
                MyLogger.Log("valid token returning");
                return token;
                }
            MyLogger.Log("token nulling");
            await localStorage.RemoveItemCustomAsync(ApiEndPoints.ApiIssuedJwt);
            return null;//as its already expired
            }

        public static async Task<T?> GetOrFetchAndSet<T>(this ILocalStorageService storage, string key, HttpClient client, string? url = null, TimeSpan? expiration = null)
            {
            //string url, mostly key & url both same in our case

            url ??= key;
            expiration ??= MaxDefaultTimespan;
            MyLogger.Log("checking if already exists:" + key);
            // Attempt to deserialize the value from localStorage
            var localResult = await storage.Get<T>(key);
            if (localResult != null)
                {
                return localResult;
                }

            MyLogger.Log("calling for url:" + url);
            // Fetch the value from the external source using the provided HttpClient and url
            var apiResult = await client.GetType<T>(url); // Assuming GetType handles the logic for making the request

            // Validate the fetched value before storing
            if (apiResult == null || !IsValid(apiResult)) // Implement your validation logic here
                {
                MyLogger.Log("response is bad for url:" + url);
                //throw new LocalStorageInvalidValueException(key);
                return default;
                }
            else
                {
                // Store the value with optional expiration (use built-in serialization)
                MyLogger.Log("Good response storing on storage for url:" + url);
                await storage.Set<T>(key, apiResult, expiration);

                MyLogger.Log("Good response stored & returning for url:" + url + " and key" + key);
                return apiResult;
                }
            }

        public static async Task Set<T>(this ILocalStorageService storage, string key, T value, TimeSpan? expiration = null)
            {
            if (!IsValid(value)) // Implement your validation logic here
                {
                MyLogger.Log("throwing error for Setting bad for key:" + key);
                throw new LocalStorageInvalidValueException(key);
                }

            // Store the value with optional expiration (use built-in serialization)
            MyLogger.Log("Setting Good for key:" + key);
            await storage.SetItemAsync(key, value);
            if (!key.EndsWith(ExpirationSuffix))
                {
                expiration ??= MaxDefaultTimespan;
                await storage.SetItemAsync(Expiration(key), DateTime.UtcNow.Add(expiration.Value).ToString());
                }
            }

        public static async Task<T?> Get<T>(this ILocalStorageService storage, string key)
            {

            string? expirationKey = Expiration(key);
            if (key.EndsWith(ExpirationSuffix)) expirationKey = null;
            try
                {
                MyLogger.Log("Getting for key:" + key);
                var result = await storage.GetItemAsync<T?>(key);
                if (result != null)
                    {
                    MyLogger.Log($"Local data exists for key:{key},checking for expiration key:{expirationKey}");
                    if (IsValid<T>(result))
                        {
                        if (string.IsNullOrEmpty(expirationKey))
                            {
                            //expirationKey null menas itself an expKey,so if key itself expiraion then dont check for another expiration key
                            }
                        else
                            {
                            var expiration = await storage.GetItemAsync<string>(expirationKey);
                            if (expiration != null)
                                {
                                MyLogger.Log($"Exists expiration key:{expirationKey}");
                                if (DateTime.TryParse(expiration, out DateTime expiryDt))
                                    {
                                    MyLogger.Log($"Able to parse expiration key:{expirationKey}");
                                    if (expiryDt > DateTime.UtcNow)//not expired
                                        {
                                        MyLogger.Log($"{key} is valid with expiration key:{expirationKey} and returning");
                                        return result;
                                        }
                                    MyLogger.Log($"Expired expiration key:{expirationKey}");
                                    }

                                MyLogger.Log($"Error,UnAble to parse expiration key:{expirationKey} & clearing expiration key");
                                //await storage.RemoveItemAsync(Expiration(key));//...
                                }
                            else
                                MyLogger.Log($"Non-Existing expiration key:{expirationKey}");
                            }
                        }
                    MyLogger.Log($"Invalid Local data exists for key:{key},so clearing");
                    await storage.RemoveItemCustomAsync(key);
                    return default;
                    }
                MyLogger.Log("No Data exists for key:" + key);
                }
            catch
                {
                MyLogger.Log($"Issue with key:{key},so clearing all");
                await storage.RemoveItemCustomAsync(key);
                }
            return default;
            }

        public static async Task RemoveItemCustomAsync(this ILocalStorageService storage, string key)
            {
            await storage.RemoveItemAsync(key);
            if (!key.EndsWith(ExpirationSuffix))
                await storage.RemoveItemAsync(Expiration(key));
            }
        private static bool IsValid<T>(T value)
            {
            return value != null;
            // Implement your custom validation logic here
            // For example, check if a property is null or empty, or if it falls within a specific range
            //return true; // Replace with your actual validation logic
            }


        }

    //public class LocalStorageValueExpiredException(string key) : Exception($"The value in localStorage with key '{key}' has expired.")
    //    {
    //    }

    public class LocalStorageInvalidValueException(string key) : Exception($"The value to be stored in localStorage with key '{key}' is invalid.")
        {
        }

    }
