using Blazored.LocalStorage;
using Blazorise;
using BlazorWebApp.Shared;
using BlazorWebApp.Shared.Services;
using MyTown.Domain;
using MyTown.RCL.CardType;
using MyTown.RCL.Town;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Cards.Commands;
using MyTown.SharedModels.Features.Cards.Queries;
using MyTown.SharedModels.Features.Towns.Commands;
using MyTown.SharedModels.Features.Towns.Queries;
using PublicCommon;
using SharedResponse;
using SharedResponse.Wrappers;
using System;
using System.Net.Http.Json;

namespace MyTown.RCL.Card
    {
    //timebeign lets go thorugh only one list where allcards of a town in one local storage,not like individual card_id

    public class CardService
        {
        private readonly HttpClient _httpClientAnonymous;
        private readonly HttpClient _httpClientAuth;
        private readonly ILocalStorageService _localStorage;
        private readonly CardTypeService _townCardTypeService;
        private readonly TownService _townService;
        readonly AuthService _authService;
        ClientConfig _clientConfig;

        readonly string _baseUrl; //= ApiEndPoints.BaseUrl(ApiEndPoints.Town);
        // private const string _baseUrl = "v1/Town";
        readonly string CardByIdUrl;// = _baseUrl + ApiEndPoints.GetById;
        //const string CardsAllKey = "Cards";//dont use direcly,always go with Cards_id OfTown
        const string CardKey = "Card";//storage format Town_id ex: Town_1 , Town_2

        List<TownCardTypeDto>? CardTypes;
        public CardService(IHttpClientFactory HttpClientFactory, ILocalStorageService localStorage, CardTypeService townCardTypeService
            , TownService townService, AuthService authService, ClientConfig clientConfig)
            {
            _httpClientAnonymous = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAnonymous);
            _httpClientAuth = HttpClientFactory.CreateClient(PublicCommon.CONSTANTS.ClientAuthorized);
            _localStorage = localStorage;
            _townCardTypeService = townCardTypeService;
            _townService = townService;
            _authService = authService;
            _clientConfig = clientConfig;
            _baseUrl = ApiEndPoints.BaseUrl(ApiEndPoints.TownCard);
            CardByIdUrl = _baseUrl + "/" + ApiEndPoints.GetById + "?";
            }
        public static string CardByIdKey(int id, string email = "")
            {
            return $"{CardKey}_{id}{email}";
            }
        readonly TimeSpan timeSpanLocalStorage = TimeSpan.FromMinutes(1);

        async Task LoadCardTypes()
            {
            CardTypes = await _townCardTypeService.GetAllTownCardTypesAsync();
            }

        public async Task<TownCardDto> GetByIdAsync(GetTownCardByIdQuery query)
            {
            //in this no localstorage of individual,instead full list...if updated remotely then ReLoad all fresh
            //todo local data is only for the sake of checking on offline case
            //will enable later,not direct all from api


            await LoadCardTypes();
            /*
            //storage key format : Town_id ex: Town_1
            var cardByIdKey = CardByIdKey(query.Id, query.UserId);
            //first check on local with expiration(internally)
            var existingLocalData = await _localStorage.GetCustom<TownCardDto>(cardByIdKey);
            if (existingLocalData != null && existingLocalData != default)
                {
                if (existingLocalData.Id == 0 || string.IsNullOrEmpty(existingLocalData.Name))
                    await _localStorage.RemoveItemCustomAsync(cardByIdKey);
                else return existingLocalData;
                }
            */
            //not existing locally,so fetching fresh
            TownCardDto? town = await _httpClientAnonymous.GetBaseResult<TownCardDto>($"{CardByIdUrl}Id={query.Id}");
            return town;
            /*
            //$"{CardByIdUrl}Id={query.Id}{(query.UserId.HasValue ? "&UserId=" + query.UserId.Value : "")}");
            //https://localhost:7195/api/v1/Town/GetById?Id=1
            if (town != null && town.Id > 0 && !string.IsNullOrEmpty(town.Name))
                {
                Console.WriteLine(town.Name);
                await _localStorage.SetCustom(cardByIdKey, town, expiration: timeSpanLocalStorage);
                return town;
                }
            else
                {
                //if existing had data,new its not then had to think whether to update or not
                return new TownCardDto();
                //await _localStorage.Set(townById, result, expiration: timeSpanLocalStorage);
                }
            */
            }

        //on any new/update card,just store locally for offline later purpose only...
        //after success modification,call town fetch with userid by reLoadByClearing=true,so it gets updated from api response
        public async Task<BaseResult<TownCardDto>> CreateUpdateTownCardAsync(CreateUpdateTownCardCommand command)
            {
            if (await _authService.IsAuthenticatedAsync())
                {
                if (string.IsNullOrEmpty(_clientConfig.Email))//this case never comes but still
                    return new BaseResult<TownCardDto>() { Success = false, Errors = [new Error(ErrorCode.AccessDenied, "Email not validated")] };
                if (command.Id == 0)
                    return await CreateTownCardAsync(command, _clientConfig.Email);
                else
                    return await UpdateTownCardAsync(command, _clientConfig.Email);
                }
            return new BaseResult<TownCardDto>() { Success = false, Errors = [new Error(ErrorCode.AccessDenied, "Modification needs Authenticated Permissions")] };
            }

        public async Task<BaseResult<TownCardDto>> CreateTownCardAsync(CreateUpdateTownCardCommand command, string email)
            {
            if (string.IsNullOrEmpty(email)) return null;//this case never comes but still
            var cardByIdKey = CardByIdKey(command.Id, email);
            var responseMessage = await _httpClientAuth.PostAsJsonAsync($"{_baseUrl}/Create", command);
            if (responseMessage != null)
                {
                var addedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
                if (addedResponse != null && addedResponse.Success && addedResponse.Data != null)
                    {//offline create handling 
                    await _localStorage.SetCustom<TownCardDto>(cardByIdKey, addedResponse.Data, expiration: timeSpanLocalStorage);
                    await _townService.GetByIdAsync(command.TownId, email, reLoadByClearingLocal: true);
                    return addedResponse;
                    }
                }
            return new BaseResult<TownCardDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }

        public async Task<BaseResult<TownCardDto>> UpdateTownCardAsync(CreateUpdateTownCardCommand command, string email)
            {
            if (string.IsNullOrEmpty(email)) return null;//this case never comes but still
            var cardByIdKey = CardByIdKey(command.Id, email);
            //todo call only if some changes exists otherwise just skip & also client side validation of allowed
            var responseMessage = await _httpClientAuth.PutAsJsonAsync($"{_baseUrl}/Update", command);
            //return await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
            if (responseMessage != null)
                {
                var updatedResponse = await responseMessage.DeserializeResponse<BaseResult<TownCardDto>>();
                if (updatedResponse != null && updatedResponse.Success && updatedResponse.Data != null)
                    {//offline create handling 

                    await _localStorage.SetCustom<TownCardDto>(cardByIdKey, updatedResponse.Data, expiration: timeSpanLocalStorage);
                    await _townService.GetByIdAsync(command.TownId, email, reLoadByClearingLocal: true);
                    return updatedResponse;
                    }
                }
            return new BaseResult<TownCardDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode
            }


        public async Task<BaseResult> DeleteTownCardAsync(int id)
            {
            if (await _authService.IsAuthenticatedAsync())
                {
                if (string.IsNullOrEmpty(_clientConfig.Email))//this case never comes but still
                    return new BaseResult<TownDto>() { Success = false, Errors = [new Error(ErrorCode.AccessDenied, "Email not validated")] };
                //no need to pass userid,but for //todo local storage handling its required
                var key = CardByIdKey(id);
                //updatedResponse parsing required as true or false
                //await _httpClientAnonymous.DeleteAsync($"{_baseUrl}/Delete?id={id}");
                var deleteResult = await _httpClientAuth.DeleteAsyncPathWithKey($"{_baseUrl}/Delete?id={id}");
                if (deleteResult != null && deleteResult.Success)
                    {
                    await _localStorage.RemoveItemCustomAsync(key);
                    return deleteResult;
                    }
                return new BaseResult<TownCardDto>() { Success = false, Data = new() };//Errors = responseMessage.StatusCode

                }
            else
                //loginmaking sure also works good
                return new BaseResult() { Success = false, Errors = [new Error(ErrorCode.AccessDenied, "Modification needs Authenticated Permissions")] };
            }
        }

    }

