﻿using Blazored.LocalStorage;
using BlazorWebApp.Shared.Services;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Towns.Commands;
using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse;
using SharedResponse.Wrappers;
using System.Net.Http.Json;

namespace MyTown.RCL.Town
    {
    public class TownService
        {
        private readonly HttpClient _httpClientAnonymous;
        private readonly HttpClient _httpClientAuth;
        private readonly ILocalStorageService _localStorage;

        readonly string _baseUrl; //= ApiEndPoints.BaseUrl(ApiEndPoints.Town);
        // private const string _baseUrl = "v1/Town";
        readonly string TownsAllUrl;// = _baseUrl + ApiEndPoints.GetAll;
        readonly string TownByIdUrl;// = _baseUrl + ApiEndPoints.GetById;
        const string TownsAllKey = "Towns";
        const string TownKey = "Town";//storage format Town_id ex: Town_1 , Town_2
        public TownService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage)
            {
            _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
            _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
            _localStorage = localStorage;
            _baseUrl = ApiEndPoints.BaseUrl(ApiEndPoints.Town);
            TownsAllUrl = _baseUrl +"/"+ ApiEndPoints.GetAll;
            TownByIdUrl = _baseUrl +"/"+ ApiEndPoints.GetById;
            }

        static string TownByIdKey(int id, Guid? userId = null)
            {
            var userIdKey = userId.HasValue ? ("_" + userId.ToString()) : "";
            return $"{TownKey}_{id}{userIdKey}";
            }
        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(5);

        //todo had to add pagination & search over api
        private async Task<PagedResponse<TownDto>?> GetTownsPaginationAsyncNotCompletedPending(GetTownsPagedListQuery query)
            {
            //todo had to pass query object
            //this fetches data for after 5 minute only,till then cache will be served for all with in browser
            var response = await _localStorage.GetOrFetchAndSet<PagedResponse<TownDto>>(TownsAllUrl, _httpClientAnonymous, url: TownsAllUrl, expiration: timeSpanLocalStorage);
            //var storageDataList = await _httpClientAnonymous.GetType<PagedResponse<TownDto>>(TownsAllUrl);
            return response;
            }

        public async Task<TownDto> GetByIdAsync(GetTownByIdQuery query)
            {
            //storage key format : Town_id ex: Town_1
            var townById = TownByIdKey(query.Id, query.UserId);
            //first check on local with expiration(internally)
            var existingLocalData = await _localStorage.Get<TownDto>(townById);
            if (existingLocalData != null) return existingLocalData;

            //not existing locally,so fetching fresh
            TownDto? town = await _httpClientAnonymous.GetType<TownDto>(TownByIdUrl);
            if (town != null)
                {
                await _localStorage.Set(townById, town, expiration: timeSpanLocalStorage);
                return town;
                }
            else
                {
                //if existing had data,new its not then had to think whether to update or not
                return new TownDto();
                //await _localStorage.Set(townById, result, expiration: timeSpanLocalStorage);
                }
            }

        public async Task<List<TownDto>> GetAllTownsAsync()
            {
            //first check on local with expiration(internally)
            var existingLocalData = await _localStorage.Get<List<TownDto>>(TownsAllKey);
            if (existingLocalData != null) return existingLocalData;

            //not existing locally,so fetching fresh
            IReadOnlyList<TownDto>? all = await _httpClientAnonymous.GetType<IReadOnlyList<TownDto>>(TownsAllUrl);
            if (all != null && all.Count > 0)
                {
                var result = all.ToList();
                await _localStorage.Set(TownsAllKey, result, expiration: timeSpanLocalStorage);
                return result;
                }
            else
                {
                var result = new List<TownDto>();
                //await _localStorage.Set(TownsAllKey, result, expiration: timeSpanLocalStorage);
                return result;
                }
            }
        public async Task<BaseResult<TownDto>> CreateUpdateTownAsync(CreateUpdateTownCommand command)
            {
            if (command.Id == 0)
                return await CreateTownAsync(command);
            else
                return await UpdateTownAsync(command);
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
        public async Task<BaseResult<TownDto>> CreateTownAsync(CreateUpdateTownCommand command)
            {
            //do minimum check of duplicate name with local storage to avoid unnecessary api calls
            var storageDataList = await _localStorage.Get<List<TownDto>>(TownsAllKey);
            if (storageDataList != null && storageDataList.Count > 0
                && storageDataList.Any(x => x.Name == command.Name))
                {
                return new BaseResult<TownDto>()
                    {
                    Success = false,
                    Data = new(),
                    Errors = [new(ErrorCode.DuplicateData)]
                    };
                }

            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
                if (addedResponse != null && addedResponse.Success && addedResponse.Data != null)
                    {//offline create handling 

                    if (storageDataList == null || storageDataList.Count == 0)
                        {//nothing exists
                        storageDataList = [addedResponse.Data];
                        }
                    else//already some data exists 
                        {
                        storageDataList.Add(addedResponse.Data);
                        //storageDataList.Sort();//not implemented yet,so dont try
                        }
                    await _localStorage.Set<List<TownDto>>(TownsAllKey, storageDataList, expiration: timeSpanLocalStorage);
                    return addedResponse;
                    }
                }
            return new BaseResult<TownDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        public async Task<BaseResult<TownDto>> UpdateTownAsync(CreateUpdateTownCommand command)
            {
            //do minimum check of duplicate name with local storage to avoid unnecessary api calls
            var storageDataList = await _localStorage.Get<List<TownDto>>(TownsAllKey);
            if (storageDataList != null && storageDataList.Count > 0
                && storageDataList.Any(x => x.Id != command.Id && x.Name == command.Name))
                {
                return new BaseResult<TownDto>()
                    {
                    Success = false,
                    Data = new(),
                    Errors = [new(ErrorCode.DuplicateData)]
                    };
                }

            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownDto>>();
                if (updatedResponse != null && updatedResponse.Success && updatedResponse.Data != null)
                    {//offline create handling 
                    if (storageDataList == null || storageDataList.Count == 0)
                        {//nothing exists
                        storageDataList = [updatedResponse.Data];
                        }
                    else//already some data exists ,remove that & add new & sort
                        {
                        storageDataList.RemoveAll(x => x.Id == updatedResponse.Data.Id);
                        storageDataList.Add(updatedResponse.Data);
                        }
                    await _localStorage.Set<List<TownDto>>(TownsAllKey, storageDataList, expiration: timeSpanLocalStorage);
                    return updatedResponse;
                    }
                }
            return new BaseResult<TownDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        public async Task<BaseResult> DeleteTownAsync(int id)
            {
            //updatedResponse parsing required as true or false
            //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
            var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
            if (deleteResult != null && deleteResult.Success)
                {
                var storageDataList = await _localStorage.Get<List<TownDto>>(TownsAllKey);
                if (storageDataList != null && storageDataList.Count > 0)
                    {
                    storageDataList.RemoveAll(x => x.Id == id);
                    await _localStorage.Set<List<TownDto>>(TownsAllKey, storageDataList, timeSpanLocalStorage);
                    }
                return deleteResult;
                }
            return new BaseResult<TownDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }
        }

    }
