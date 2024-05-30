using Blazored.LocalStorage;
using BlazorWebApp.Shared.Services;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Cards.Commands;
using MyTown.SharedModels.Features.Cards.Queries;
using PublicCommon;



//using Nextended.Core.Extensions;
using SharedResponse.Wrappers;
using System.Net.Http.Json;

namespace MyTown.RCL.Card
    {
    public class TownCardService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage)
        {
        private readonly HttpClient _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
        private readonly HttpClient _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly string _baseUrl = "v1/TownCard";
        private const string TownCardsKey = "v1/TownCard/GetPagedList";//for paged query
                                                                       // private const string TownCardsAllKey = "v1/TownCard/GetAllPagedList";//for all paged query

        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(5);
        public async Task<PagedResponse<TownCardDto>?> GetTownCardsAsync(GetTownCardsPagedListQuery query)
            {
            //todo had to pass query object
            //this fetches data for after 5 minute only,till then cache will be served for all with in browser
            var response = await _localStorage.GetOrFetchAndSet<PagedResponse<TownCardDto>>(TownCardsKey, _httpClientAnonymous, url: TownCardsKey, expiration: timeSpanLocalStorage);
            //var storageDataList = await _httpClientAnonymous.GetType<PagedResponse<TownCardDto>>(TownCardsKey);
            return response;
            }
        public async Task<BaseResult<TownCardDto>> CreateUpdateTownCardAsync(CreateUpdateTownCardCommand command)
            {
            if (command.Id == 0)
                return await CreateTownCardAsync(command);
            else
                return await UpdateTownCardAsync(command);
            }


        public async Task<BaseResult<TownCardDto>> CreateTownCardAsync(CreateUpdateTownCardCommand command)
            {
            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardDto>>(TownCardsKey);
                if (storageDataList != null && addedResponse != default && addedResponse.Data != default && storageDataList.Data != null)
                    {
                    storageDataList.Data.Add(addedResponse.Data);
                    await _localStorage.Set<PagedResponse<TownCardDto>>(TownCardsKey, storageDataList, timeSpanLocalStorage);
                    return addedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult<TownCardDto>> UpdateTownCardAsync(CreateUpdateTownCardCommand command)
            {
            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
                MyLogger.Log($"{_baseUrl}/update result cameback");
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardDto>>(TownCardsKey);
                MyLogger.Log($"{_baseUrl} existing data extracted");
                if (storageDataList != null && updatedResponse != default && updatedResponse.Data != default && storageDataList.Data != null)
                    {
                    MyLogger.Log($"{_baseUrl} existing data extracted & updating cache");
                    storageDataList.Data.RemoveAll(x => x.Id == updatedResponse.Data.Id);
                    storageDataList.Data.Add(updatedResponse.Data);
                    MyLogger.Log($"{_baseUrl} updating list");
                    await _localStorage.Set<PagedResponse<TownCardDto>>(TownCardsKey, storageDataList, timeSpanLocalStorage);
                    return updatedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult> DeleteTownCardAsync(int id)
            {
            //updatedResponse parsing required as true or false
            //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
            var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
            if (deleteResult != null && deleteResult.Success)
                {
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardDto>>(TownCardsKey);
                if (storageDataList != null && storageDataList.Data != null)
                    {
                    storageDataList.Data.RemoveAll(x => x.Id == id);
                    await _localStorage.Set<PagedResponse<TownCardDto>>(TownCardsKey, storageDataList, timeSpanLocalStorage);
                    }
                }
            return deleteResult;
            }
        }

    }

