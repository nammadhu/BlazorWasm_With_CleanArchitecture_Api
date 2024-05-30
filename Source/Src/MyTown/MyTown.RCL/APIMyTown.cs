//using Azure;

//using Dto;
using MyTown.SharedModels.DTOs;
using SharedResponse;
using System.Net.Http.Json;

namespace MyTown.RCL;

//for town
public static class APIMyTown
    {
    const string prefix = "v1/";
    //common settings in other file
    public static async Task<TownDto?> TownGet(this HttpClient client, int townId, Guid? UserId)
        {
        HttpResponseMessage response;
        //if (UserId != null)
        //    response = await client.GetAsync($"{prefix}{ApiEndPoints.Town}/{townId}/{UserId}");
        //else
        response = await client.GetAsync($"{prefix}{ApiEndPoints.Town}/{townId}");
        if (response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
            return await response.Content.ReadFromJsonAsync<TownDto?>();
            }
        return null;
        }

    public static async Task<int> TownPost(this HttpClient client, TownDto Model, Guid userId)
        {
        //todo mostly this userid is of no use,bcz api side does the validation
        var response = await client.PostAsJsonAsync(prefix + ApiEndPoints.Town, Model);
        //show loading symbol
        if (response.IsSuccessStatusCode)
            {
            return await response.Content.ReadFromJsonAsync<int>();
            }
        return 0;
        }

    }

