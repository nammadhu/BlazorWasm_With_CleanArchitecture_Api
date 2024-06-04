using Blazored.LocalStorage;
using PublicCommon;
using SharedResponse;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace BlazorWebApp.Shared.Services
    {
    //Notusing 
    public class HttpAnonymousOptionalUserInterceptorNotUsing(AuthService authService, ILocalStorageService localStorageService) : DelegatingHandler
        {

        //this gets intercepted in calling Anonymous endpoints 
        //must be httpclient of name "AnonymousClient" then it gets auto intercepted

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
            var jwtTokenApiIssued = await GetIdToken();
            if (!string.IsNullOrEmpty(jwtTokenApiIssued))
                {
                request.Headers.Authorization = new AuthenticationHeaderValue(CONSTANTS.Bearer, jwtTokenApiIssued);
                }
            // Encrypt the content
            //string encryptedContent = Encryption.EncryptString("your-shared-key", "madhu here kanri");
            if (!request.Headers.Contains(AnonymousHeader.Key))
                {
                request.Headers.Add(AnonymousHeader.Key, AnonymousHeader.Value);
                }

            return await base.SendAsync(request, cancellationToken);

            }
        private async Task<string> GetIdToken()
            {
            var token = await authService.GetIdTokenAsync();
            if (!string.IsNullOrEmpty(token))
                {
                var jwtTokenApiIssued = await localStorageService.GetItemAsStringAsync(ApiEndPoints.ApiIssuedJwt);
                //todo check for when its stored & expiration
                //jwtTokenApiIssued = jwtTokenApiIssued ?? "".Trim().Trim('"').Replace("\"", "");//not working
                jwtTokenApiIssued = jwtTokenApiIssued ?? "".Trim().Trim('"');
                jwtTokenApiIssued = Regex.Replace(jwtTokenApiIssued, "\"", "");
                return jwtTokenApiIssued;
                }
            return null;
            }
        }

    }
