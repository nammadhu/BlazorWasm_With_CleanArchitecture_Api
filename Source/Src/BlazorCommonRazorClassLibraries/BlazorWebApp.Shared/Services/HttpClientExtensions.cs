using SharedResponse.Wrappers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorWebApp.Shared.Services
    {
    public static class HttpClientExtensions
        {

        public static async Task<T?> GetBaseResult<T>(this HttpClient client, string key)
            {
            if (client != null)
                {
                try
                    {
                    var response = await client.GetAsync(key);
                    if (response.IsSuccessStatusCode)
                        {
                        var result = await DeserializeResponse<BaseResult<T>>(response);
                        return result != null && result.Success ? result.Data : default;
                        }
                    }
                catch (Exception ex)
                    {
                    // Handle API call failure (log error, return default(T), throw exception)
                    //Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.ToString()}");//including line number full details
                    Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.Message}");
                    //return default(T); // Or throw a specific exception
                    }
                }
            return default;
            }
        public static async Task<List<T>?> GetPagedResponse<T>(this HttpClient client, string key)
            {
            if (client != null)
                {
                try
                    {
                    var response = await client.GetAsync(key);
                    if (response.IsSuccessStatusCode)
                        {
                        var result = await DeserializeResponse<PagedResponse<T>>(response);
                        return result != null && result.Success ? result.Data : default;
                        }
                    }
                catch (Exception ex)
                    {
                    // Handle API call failure (log error, return default(T), throw exception)
                    //Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.ToString()}");//including line number full details
                    Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.Message}");
                    //return default(T); // Or throw a specific exception
                    }
                }
            return default;
            }
        public static async Task<T?> GetType<T>(this HttpClient client, string key)
            {
            if (client != null)
                {
                try
                    {
                    var response = await client.GetAsync(key);
                    return await DeserializeResponse<T>(response);
                    }
                catch (Exception ex)
                    {
                    // Handle API call failure (log error, return default(T), throw exception)
                    //Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.ToString()}");//including line number full details
                    Console.Error.WriteLine($"Error fetching data for key '{key}': {ex.Message}");
                    //return default(T); // Or throw a specific exception
                    }
                }
            return default;
            }

        public static async Task<BaseResult> DeleteAsyncPathWithKey(this HttpClient client, string pathWithKey)
            {
            //result parsing required as true or false
            var response = await client.DeleteAsync(pathWithKey);
            return await DeserializeResponse<BaseResult>(response);
            }
        public static async Task<T?> DeserializeResponse<T>(this HttpResponseMessage response)
            {
            if (response == null || !response.IsSuccessStatusCode) return default;
            //todo there might be a chance content read in try ,then catch block might fail...so had to handle not sure
            try
                {
                //return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync());

                //var res = await response.Content.ReadAsStringAsync();
                //var t1 = JsonSerializer.Deserialize<T>(res);
                //return t1;
                //above is for testing but slow so
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
                }
            catch (JsonException)
                {
                return (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T)); // Consider type safety implications
                }
            catch (Exception ex)
                {
                Console.WriteLine(ex.Message);
                }
            return default;
            }
        }

    }
