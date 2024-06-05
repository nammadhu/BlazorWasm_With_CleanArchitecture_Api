using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PublicCommon;
using SharedResponse;
using SharedResponse.Wrappers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlazorWebApp.Shared.Services
    {
    public class AuthService(
        AuthenticationStateProvider authStateProvider, IJSRuntime JSRuntime, ILocalStorageService localStorageService,
        IHttpClientFactory httpClientFactory, NavigationManager navigationManager, ClientConfig clientConfig)
        {
        //  private string _cachedIdToken;//todo not used
        readonly IJSRuntime _JSRuntime = JSRuntime;

        public async Task<bool> IsAuthenticatedAsync()//just makes validation of google login
            {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            MyLogger.Log($"Authstate is null:{authState == null}");
            bool result = authState?.User?.Identity?.IsAuthenticated ?? false;
            MyLogger.Log($"Authstate is result:{result}");
            if (result)
                {
                MyLogger.Log($"Authstate email checking in token");
                var email = authState!.User.Claims?.Single(c => c.Type == "email")?.Value;
                if (email != null)
                    {
                    MyLogger.Log($"Setting clientconfig ,first checking");
                    if (string.IsNullOrEmpty(clientConfig.Email) || clientConfig.Email != email)
                        {
                        MyLogger.Log($"Setting clientconfig email as" + email);
                        clientConfig.EmailSet(email);
                        // await localStorageService.SetItemAsStringAsync(CONSTANTS.Email, email);//not required
                        }
                    }
                else
                    MyLogger.Log($"Authstate email null");

                }
            else
                {
                MyLogger.Log("not authenticated");
                }
            return result;
            }
        public async Task<bool> AuthorizeRoles()//api role management starts here only,so always required before operation
            {
            if (await IsAuthenticatedAsync() && clientConfig.IsAdmin == null)//isadmin null means roles not yet loaded
                {
                await TokenExtract();//this makes db entry even if its not registered
                var response = await localStorageService.GetApiTokenFromLocalStorage();
                //here if null then fetch again had to be done
                if (response != null)
                    {
                    string? userId = response?.Claims?.Single(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid id))
                        {
                        clientConfig.UserIdSet(id);
                        //await _localStorageService.SetItemAsStringAsync(CONSTANTS.UserId, userId);
                        //lets avoid exposing userid to clients easily
                        }
                    }
                clientConfig.RolesSet(response?.Claims);
                return true;
                }
            return false;
            }

        public async Task LoginUserMakingSure(string returnUrl = "/Town/CardTypes")
            {

            if (!await IsAuthenticatedAsync())
                {
                if (!string.IsNullOrEmpty(returnUrl))
                    {//NavigationManager.NavigateTo("/authentication/login?returnUrl=/TownCardTypes");
                    navigationManager.NavigateTo("/authentication/login?returnUrl=" + returnUrl);
                    }
                //currently redirect to url is not working
                navigationManager.NavigateTo("/authentication/login");
                }

            }
        public async Task TokenExtract()
            {
            //todo currently only google handled, if more added then had to add more for the same
            if (await IsAuthenticatedAsync() && !clientConfig.ApiTokenFetching && !clientConfig.ApiTokenFetched)
                {

                var apiToken = await localStorageService.GetApiTokenFromLocalStorage();
                if (!string.IsNullOrEmpty(clientConfig.Email) && apiToken != null && apiToken?.Claims?.Single(c => c.Type == ClaimTypes.Email)?.Value == clientConfig.Email)
                    {
                    MyLogger.Log("re using exisitng token");
                    return;
                    }
                MyLogger.Log("lets clear existing token if any");
                await localStorageService.RemoveItemAsync(ApiEndPoints.ApiIssuedJwt);//
                MyLogger.Log("Fetching fresh token,checking autToken existence");
                clientConfig.ApiTokenFetchingUpdate(true);
                var authIdToken = await GetIdTokenAsync();
                if (!string.IsNullOrEmpty(authIdToken))
                    {
                    MyLogger.Log("Fetching fresh token using exisiting autToken");
                    var clientAnonymous = httpClientFactory.CreateClient(ApiEndPoints.ClientAnonymous);
                    //authIdToken = authIdToken ?? "".Trim().Trim('"').Replace("\"", "");//not working

                    authIdToken = authIdToken ?? "".Trim().Trim('"');
                    authIdToken = Regex.Replace(authIdToken, "\"", "");
                    MyLogger.Log($"authIdToken is :'{authIdToken ?? "empty"}'");
                    clientAnonymous.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(CONSTANTS.Bearer, authIdToken);

                    var json = $"{{\"AppKey\": \"{CONSTANTS.MyTownApp}\"}}";
                    // var json = $"{{\"AppKey\": \"{PublicCommon.CONSTANTS.MyTownApp } App Key\"";//this is just for testing but functionally not much important
                    var stringContent = new StringContent(json, Encoding.UTF8, CONSTANTS.ApplicationJson);
                    //if ( _tokenstate.Value.ApiTokenFetched) return;
                    var response = await clientAnonymous.PostAsync(ApiEndPoints.TokenExtractionPostGoogle, stringContent);


                    MyLogger.Log("api issued jwt token response is:" + response);
                    if (response.IsSuccessStatusCode)
                        {
                        var fullResult = await response.DeserializeResponse<BaseResult<AuthenticationResponse>>();
                        var jwtToken = fullResult?.Data?.JWToken; //await response.Content.ReadAsStringAsync();//this is full BaseResult
                        if (string.IsNullOrEmpty(jwtToken))
                            {
                            MyLogger.Log("api issued jwt token is null,so removing if any");
                            await localStorageService.RemoveItemAsync(ApiEndPoints.ApiIssuedJwt);

                            }
                        else
                            {
                            MyLogger.Log("api issued jwt token is " + jwtToken);
                            await localStorageService.SetCustom<string>(ApiEndPoints.ApiIssuedJwt, jwtToken);
                            // UserTokenStore the JWT token and use it for subsequent API requests
                            }

                        MyLogger.Log("extracted userinfo");
                        }
                    else
                        {
                        MyLogger.Log("no call due to Service Issue:" + response.StatusCode);
                        }
                    }
                else
                    {
                    MyLogger.Log("AuthToken is null,so not fetching Api Token ");
                    }
                clientConfig.ApiTokenFetchedUpdate(true);
                }
            }


        private async Task<string> GetUserIdTokenFromBrowserSession()
            {
            //TODO if user is using google authentication then google format, something else then that particular had to be considered
            string userDataKey = $"oidc.user:https://accounts.google.com/:{clientConfig.Settings.Authentications.FirstOrDefault(x => x.Type == CONSTANTS.Auth.ExternalProviders.Google).ClientId}";
            // var userData0 = await JSRuntime.InvokeAsync<UserData>("sessionStorage.getItem", userDataKey);
            // MyLogger.Log("tok:"+userData0.id_token);//this wont work,so get string first then parse as below
            var userDataString = await _JSRuntime.InvokeAsync<string>("sessionStorage.getItem", userDataKey);
            return userDataString;
            }
        public async Task<string?> GetIdTokenAsync()
            {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                var userDataString = await GetUserIdTokenFromBrowserSession();
                //MyLogger.Log("val:" + userDataString);
                if (userDataString != null)
                    {
                    try
                        {
                        var userData = JsonSerializer.Deserialize<UserData>(userDataString);
                        if (userData == null)
                            {
                            MyLogger.Log("deserialized userdata is null");
                            return null;
                            }
                        // MyLogger.Log("val2:" + userData.id_token);
                        return userData.id_token;
                        }
                    catch (Exception ex)
                        {
                        MyLogger.Log("Error parsing user data:" + ex.ToString());
                        //return null; // Or throw an exception
                        }
                    }
                else
                    {
                    MyLogger.Log("Google storage key fetching issue or not exists");
                    //return null; // Or throw an exception
                    }
                }
            else
                {
                MyLogger.Log("erro: no validation exists");
                }
            return null;
            }
        }
    public class Profile//wrt google
        {
        public string azp { get; set; }
        public string sub { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string jti { get; set; }
        }

    public class UserData//wrt google
        {
        public string id_token { get; set; }
        public string scope { get; set; }
        public Profile profile { get; set; }
        }
    }