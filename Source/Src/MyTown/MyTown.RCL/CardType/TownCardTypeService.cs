using Blazored.LocalStorage;
using BlazorWebApp.Shared.Services;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.CardTypes.Commands;
using MyTown.SharedModels.Features.CardTypes.Queries;
using PublicCommon;



//using Nextended.Core.Extensions;
using SharedResponse.Wrappers;
using System.Net.Http.Json;

namespace MyTown.RCL.CardType
    {
    public class TownCardTypeService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage)
        {
        private readonly HttpClient _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
        private readonly HttpClient _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly string _baseUrl = "v1/TownCardType";
        private const string TownCardTypesKey = "v1/TownCardType/GetPagedList";//for paged query
                                                                               // private const string TownCardTypesAllKey = "v1/TownCardType/GetAllPagedList";//for all paged query

        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(5);
        public async Task<PagedResponse<TownCardTypeDto>?> GetTownCardTypesAsync(GetTownCardTypeMasterDatasPagedListQuery query)
            {
            //todo had to pass query object
            //this fetches data for after 5 minute only,till then cache will be served for all with in browser
            var response = await _localStorage.GetOrFetchAndSet<PagedResponse<TownCardTypeDto>>(TownCardTypesKey, _httpClientAnonymous, url: TownCardTypesKey, expiration: timeSpanLocalStorage);
            //var storageDataList = await _httpClientAnonymous.GetType<PagedResponse<TownCardTypeDto>>(TownCardTypesKey);
            return response;
            }
        public async Task<BaseResult<TownCardTypeDto>> CreateUpdateTownCardTypeAsync(CreateUpdateTownCardTypeMasterDataCommand command)
            {
            if (command.Id == 0)
                return await CreateTownCardTypeAsync(command);
            else
                return await UpdateTownCardTypeAsync(command);
            }


        public async Task<BaseResult<TownCardTypeDto>> CreateTownCardTypeAsync(CreateUpdateTownCardTypeMasterDataCommand command)
            {
            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardTypeDto>>(TownCardTypesKey);
                if (storageDataList != null && addedResponse != default && addedResponse.Data != default && storageDataList.Data != null)
                    {
                    storageDataList.Data.Add(addedResponse.Data);
                    await _localStorage.Set<PagedResponse<TownCardTypeDto>>(TownCardTypesKey, storageDataList, timeSpanLocalStorage);
                    return addedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult<TownCardTypeDto>> UpdateTownCardTypeAsync(CreateUpdateTownCardTypeMasterDataCommand command)
            {
            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
                MyLogger.Log($"{_baseUrl}/update result cameback");
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardTypeDto>>(TownCardTypesKey);
                MyLogger.Log($"{_baseUrl} existing data extracted");
                if (storageDataList != null && updatedResponse != default && updatedResponse.Data != default && storageDataList.Data != null)
                    {
                    MyLogger.Log($"{_baseUrl} existing data extracted & updating cache");
                    storageDataList.Data.RemoveAll(x => x.Id == updatedResponse.Data.Id);
                    storageDataList.Data.Add(updatedResponse.Data);
                    MyLogger.Log($"{_baseUrl} updating list");
                    await _localStorage.Set<PagedResponse<TownCardTypeDto>>(TownCardTypesKey, storageDataList, timeSpanLocalStorage);
                    return updatedResponse;
                    }
                }
            return null;
            }

        public async Task<BaseResult> DeleteTownCardTypeAsync(int id)
            {
            //updatedResponse parsing required as true or false
            //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
            var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
            if (deleteResult != null && deleteResult.Success)
                {
                var storageDataList = await _localStorage.Get<PagedResponse<TownCardTypeDto>>(TownCardTypesKey);
                if (storageDataList != null && storageDataList.Data != null)
                    {
                    storageDataList.Data.RemoveAll(x => x.Id == id);
                    await _localStorage.Set<PagedResponse<TownCardTypeDto>>(TownCardTypesKey, storageDataList, timeSpanLocalStorage);
                    }
                }
            return deleteResult;
            }
        }

    }

