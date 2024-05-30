using Blazored.LocalStorage;
using BlazorWebApp.Shared.Services;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Towns.Commands;
using MyTown.SharedModels.Features.Towns.Queries;
using PublicCommon;



//using Nextended.Core.Extensions;
using SharedResponse.Wrappers;
using System.Net.Http.Json;

namespace MyTown.RCL.Town
    {
    public class TownService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage)
        {
        private readonly HttpClient _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
        private readonly HttpClient _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly string _baseUrl = "v1/Town";
        private const string TownsKey = "v1/Town/GetPagedList";//for paged query
                                                               // private const string TownsAllKey = "v1/Town/GetAllPagedList";//for all paged query

        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(5);
        public async Task<PagedResponse<TownDto>?> GetTownsAsync(GetTownsPagedListQuery query)
            {
            //todo had to pass query object
            //this fetches data for after 5 minute only,till then cache will be served for all with in browser
            var response = await _localStorage.GetOrFetchAndSet<PagedResponse<TownDto>>(TownsKey, _httpClientAnonymous, url: TownsKey, expiration: timeSpanLocalStorage);
            //var storageDataList = await _httpClientAnonymous.GetType<PagedResponse<TownDto>>(TownsKey);
            return response;
            }
        public async Task<BaseResult<TownDto>> CreateUpdateTownAsync(CreateUpdateTownCommand command)
            {
            if (command.Id == 0)
                return await CreateTownAsync(command);
            else
                return await UpdateTownAsync(command);
            }


        public async Task<BaseResult<TownDto>> CreateTownAsync(CreateUpdateTownCommand command)
            {
            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
                var storageDataList = await _localStorage.Get<PagedResponse<TownDto>>(TownsKey);
                if (storageDataList != null && addedResponse != default && addedResponse.Data != default && storageDataList.Data != null)
                    {
                    storageDataList.Data.Add(addedResponse.Data);
                    await _localStorage.Set<PagedResponse<TownDto>>(TownsKey, storageDataList, timeSpanLocalStorage);
                    return addedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult<TownDto>> UpdateTownAsync(CreateUpdateTownCommand command)
            {
            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
                MyLogger.Log($"{_baseUrl}/update result cameback");
                var storageDataList = await _localStorage.Get<PagedResponse<TownDto>>(TownsKey);
                MyLogger.Log($"{_baseUrl} existing data extracted");
                if (storageDataList != null && updatedResponse != default && updatedResponse.Data != default && storageDataList.Data != null)
                    {
                    MyLogger.Log($"{_baseUrl} existing data extracted & updating cache");
                    storageDataList.Data.RemoveAll(x => x.Id == updatedResponse.Data.Id);
                    storageDataList.Data.Add(updatedResponse.Data);
                    MyLogger.Log($"{_baseUrl} updating list");
                    await _localStorage.Set<PagedResponse<TownDto>>(TownsKey, storageDataList, timeSpanLocalStorage);
                    return updatedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult> DeleteTownAsync(int id)
            {
            //updatedResponse parsing required as true or false
            //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
            var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
            if (deleteResult != null && deleteResult.Success)
                {
                var storageDataList = await _localStorage.Get<PagedResponse<TownDto>>(TownsKey);
                if (storageDataList != null && storageDataList.Data != null)
                    {
                    storageDataList.Data.RemoveAll(x => x.Id == id);
                    await _localStorage.Set<PagedResponse<TownDto>>(TownsKey, storageDataList, timeSpanLocalStorage);
                    }
                }
            return deleteResult;
            }
        }

    }

