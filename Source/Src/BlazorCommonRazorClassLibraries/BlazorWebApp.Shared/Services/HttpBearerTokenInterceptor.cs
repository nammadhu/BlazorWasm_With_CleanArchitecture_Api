using Blazored.LocalStorage;
using PublicCommon;
using SharedResponse;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace BlazorWebApp.Shared.Services
    {
    public class HttpBearerTokenInterceptor(AuthService authService, ILocalStorageService localStorageService) : DelegatingHandler
        {
        //this gets intercepted in calling auth endpoints & includes ApiIssuedJwt
        //must be httpclient of name "AuthClient" then it gets auto intercepted
        readonly AuthService _authService = authService;
        readonly ILocalStorageService _localStorageService = localStorageService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
            var token = await _authService.GetIdTokenAsync();
            if (!string.IsNullOrEmpty(token))
                {
                var jwtTokenApiIssued = await _localStorageService.GetItemAsStringAsync(ApiEndPoints.ApiIssuedJwt, cancellationToken);
                //todo check for when its stored & expiration
                //jwtTokenApiIssued = jwtTokenApiIssued ?? "".Trim().Trim('"').Replace("\"", "");//not working
                jwtTokenApiIssued = jwtTokenApiIssued ?? "".Trim().Trim('"');
                jwtTokenApiIssued = Regex.Replace(jwtTokenApiIssued, "\"", "");


                //Console.WriteLine($"tkn is {jwtTokenApiIssued}");
                if (!string.IsNullOrEmpty(jwtTokenApiIssued))
                    {
                    request.Headers.Authorization = new AuthenticationHeaderValue(CONSTANTS.Bearer, jwtTokenApiIssued);
                    //request.Headers.Add("Authorization", "Bearer " + jwtTokenApiIssued);

                    // Encrypt the content
                    //string encryptedContent = Encryption.EncryptString("your-shared-key", "madhu here kanri");

                    if (!request.Headers.Contains(CONSTANTS.AppKeyName))
                        {
                        request.Headers.Add(CONSTANTS.AppKeyName, CONSTANTS.MyTownAppKeyAuth);
                        }


                    return await base.SendAsync(request, cancellationToken);
                    }
                }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized) // Return Unauthorized
                {
                ReasonPhrase = $"No token available for request {request.RequestUri?.ToString()}" // SettingsUpdate custom reason phrase (optional)
                };

            }
        }

    }
