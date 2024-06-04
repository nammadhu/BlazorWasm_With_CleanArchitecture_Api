using Blazored.LocalStorage;
using BlazorWebApp.Shared;
using BlazorWebApp.Shared.Services;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.CardTypes.Commands;
using MyTown.SharedModels.Features.CardTypes.Queries;
using PublicCommon;
using SharedResponse;
using SharedResponse.Wrappers;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyTown.RCL.CardType
    {
    public class CardTypeService
        {
        private readonly HttpClient _httpClientAnonymous;
        private readonly HttpClient _httpClientAuth;
        private readonly ILocalStorageService _localStorage;
        //IHttpClientFactory _HttpClientFactory;
        //readonly AuthService _authService;
        readonly string _baseUrl; 
        readonly string TownCardTypesAllUrl;// = _baseUrl + ApiEndPoints.GetAll;
        const string TownCardTypesAllKey = "TownCardTypes";
        public CardTypeService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage)//, AuthService authService)
            {
      //      _HttpClientFactory = HttpClientFactory;
            _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
            _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
            _localStorage = localStorage;
    //        _authService = authService;
            _baseUrl = ApiEndPoints.BaseUrl(ApiEndPoints.TownCardType);
            TownCardTypesAllUrl = _baseUrl + "/" + ApiEndPoints.GetAll;
            }

        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(60);
    
        public async Task<List<TownCardTypeDto>> GetAllTownCardTypesAsync()
            {
            //first check on local with expiration(internally)
            var existingLocalData = await _localStorage.GetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey);
            if (existingLocalData != null) return existingLocalData;

            //not existing locally,so fetching fresh
            IReadOnlyList<TownCardTypeDto>? all = await _httpClientAnonymous.GetType<IReadOnlyList<TownCardTypeDto>>(TownCardTypesAllUrl);
            if (all != null && all.Count > 0)
                {
                var result = all.ToList();
                await _localStorage.SetCustom(TownCardTypesAllKey, result, expiration: timeSpanLocalStorage);
                return result;
                }
            else
                {
                var result = new List<TownCardTypeDto>();
                //await _localStorage.Set(TownCardTypesAllKey, result, expiration: timeSpanLocalStorage);
                return result;
                }
            }
        public async Task<BaseResult<TownCardTypeDto>> CreateUpdateTownCardTypeAsync(CreateUpdateTownCardTypeCommand command)
            {
            if (command.Id == 0)
                return await CreateTownCardTypeAsync(command);
            else
                return await UpdateTownCardTypeAsync(command);
            }
        /*local storage on Craete/Update logic
        On Create,
            A.if existing is null or empty ,=>create new list and add new item return
            B.else(already has data), => fetch existing list ,plus add new item and sort return

        On Update,
            A.if exisitng is null or empty, add new list with new item or fetch full list
            B.else extract list, remove all existing ids ,then insert new item,sort & return

        On Delete,
            A.if existing is null, return
            B.else remove all matching conditions
            
            */
        public async Task<BaseResult<TownCardTypeDto>> CreateTownCardTypeAsync(CreateUpdateTownCardTypeCommand command)
            {
            //do minimum check of duplicate name with local storage to avoid unnecessary api calls
            var storageDataList = await _localStorage.GetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey);
            if (storageDataList != null && storageDataList.Count > 0
                && storageDataList.Any(x => x.Name == command.Name))
                {
                return new BaseResult<TownCardTypeDto>()
                    {
                    Success = false,
                    Data = new(),
                    Errors = [new(ErrorCode.DuplicateData)]
                    };
                }

            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
                if (addedResponse != null && addedResponse.Success && addedResponse.Data != null)
                    {//offline create handling 

                    if (storageDataList == null || storageDataList.Count == 0)
                        {//nothing exists
                        storageDataList = [addedResponse.Data];
                        }
                    else//already some data exists 
                        {
                        storageDataList.Insert(0, addedResponse.Data);
                        }
                    await _localStorage.SetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey, storageDataList, expiration: timeSpanLocalStorage);
                    return addedResponse;
                    }
                }
            return new BaseResult<TownCardTypeDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        public async Task<BaseResult<TownCardTypeDto>> UpdateTownCardTypeAsync(CreateUpdateTownCardTypeCommand command)
            {
            //do minimum check of duplicate name with local storage to avoid unnecessary api calls
            var storageDataList = await _localStorage.GetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey);
            if (storageDataList != null && storageDataList.Count > 0
                && storageDataList.Any(x => x.Id != command.Id && x.Name == command.Name))
                {
                return new BaseResult<TownCardTypeDto>()
                    {
                    Success = false,
                    Data = new(),
                    Errors = [new(ErrorCode.DuplicateData)]
                    };
                }

            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardTypeDto>>();
                if (updatedResponse != null && updatedResponse.Success && updatedResponse.Data != null)
                    {//offline create handling 
                    if (storageDataList == null || storageDataList.Count == 0)
                        {//nothing exists
                        storageDataList = [updatedResponse.Data];
                        }
                    else//already some data exists ,remove that & add new & sort
                        {
                        var index = storageDataList.FindIndex(t => t.Id == updatedResponse.Data.Id);
                        storageDataList[index] = updatedResponse.Data;
                        ListExtensions.UpdateAndMoveToFront(storageDataList, index, _ => { });
                        }
                    await _localStorage.SetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey, storageDataList, expiration: timeSpanLocalStorage);
                    return updatedResponse;
                    }
                }
            return new BaseResult<TownCardTypeDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        public async Task<BaseResult> DeleteTownCardTypeAsync(int id)
            {
            //updatedResponse parsing required as true or false
            //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
            var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
            if (deleteResult != null && deleteResult.Success)
                {
                var storageDataList = await _localStorage.GetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey);
                if (storageDataList != null && storageDataList.Count > 0)
                    {
                    storageDataList.RemoveAll(x => x.Id == id);
                    await _localStorage.SetCustom<List<TownCardTypeDto>>(TownCardTypesAllKey, storageDataList, timeSpanLocalStorage);
                    }
                return deleteResult;
                }
            return new BaseResult<TownCardTypeDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        //todo had to add pagination & search over api
        private async Task<PagedResponse<TownCardTypeDto>?> GetTownCardTypesPaginationAsyncNotCompletedPending(GetTownCardTypesPagedListQuery query)
            {
            //todo had to pass query object
            //this fetches data for after 5 minute only,till then cache will be served for all with in browser
            var response = await _localStorage.GetOrFetchAndSet<PagedResponse<TownCardTypeDto>>(TownCardTypesAllUrl, _httpClientAnonymous, url: TownCardTypesAllUrl, expiration: timeSpanLocalStorage);
            //var storageDataList = await _httpClientAnonymous.GetType<PagedResponse<TownCardTypeDto>>(TownCardTypesAllUrl);
            return response;
            }
        }

    }

