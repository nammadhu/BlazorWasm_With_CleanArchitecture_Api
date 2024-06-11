using CleanArchitecture.Application.DTOs.Account.Requests;
using CleanArchitecture.Application.Helpers;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Interfaces.UserInterfaces;
using Google.Apis.Auth;
using SharedResponse.Wrappers;
using CleanArchitecture.Infrastructure.Identity.Models;
using CleanArchitecture.Infrastructure.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PublicCommon;
using SharedResponse;
using SharedResponse.Wrappers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Identity.Services;

//social login google and all
public partial class AccountServices
    {

    private async Task<BaseResult<AuthenticationResponse>> AuthenticateWithGoogle(string googleJwtToken)
        {
        //steps
        //1. validate google jwt & fetch payload
        //2.for payload email check if account exists or not
        //3. if not exists then creates & logins(of Google type instead of local)
        //4. creates new jwt of user fetched from db
        //5. extracts roles of user from db
        //6. returns newly created jwt for further

        //since token validation happens at jwt level only so not using this ,instead using GetJwtByCreateAccountOrFetchWithSocialAsync
        var payload = await ValidateGoogleJwtToken(googleJwtToken);
        if (payload == null)
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, "Invalid Google token."));
            }
        return await GetJwtByCreateAccountOrFetchWithSocialAsync(payload.Email, payload.Email, payload.Name, payload.Subject, CONSTANTS.Auth.ExternalProviders.Google).ConfigureAwait(false);
        }

    public async Task<BaseResult<AuthenticationResponse>> GetJwtByCreateAccountOrFetchWithSocialAsync(string userName, string email, string name, string subject, string loginProvider = CONSTANTS.Auth.ExternalProviders.Google)
        {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            {
            user = new ApplicationUser { UserName = userName ?? email, Email = email, Name = name ?? email };
            var identityResult = await userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
                {
                return new BaseResult<AuthenticationResponse>(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));
                }

            await userManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, subject, loginProvider));
            }

        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
        // user.Roles = [.. rolesList];//this is not required
        //keep this earlier than jwt token generation
        var jwToken = await GenerateJwtToken(user);


        AuthenticationResponse response = new AuthenticationResponse
            {
            Id = user.Id.ToString(),
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
            Email = user.Email,
            UserName = user.UserName,
            Roles = rolesList.ToList(),
            IsVerified = user.EmailConfirmed,
            };

        return new BaseResult<AuthenticationResponse>(response);
        }

    /// <summary>
    /// validate google jwt and return payloads
    /// </summary>
    /// <param name="googleJwtToken"></param>
    /// <returns></returns>
    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleJwtToken(string googleJwtToken)
        {
        try
            {
            var google = appConfigurations.AppSettings.Authentications.Find(x => x.Type == CONSTANTS.Auth.ExternalProviders.Google);
            var settings = new GoogleJsonWebSignature.ValidationSettings()
                {

                //Audience = new List<string>() { configuration["GoogleAuthSettings:ClientId"] }
                Audience = new List<string>() { google.ClientId }
                //like "28358123213176-v7o7a3vs9sd269i8qtknjua8kddmine1.apps.googleusercontent.com"  //remove this
                };

            var payload = await GoogleJsonWebSignature.ValidateAsync(googleJwtToken, settings);
            return payload;
            }
        catch (Exception e)
            {
            Console.WriteLine(e.ToString());
            // The token is invalid
            return null;
            }
        }


    public async Task<BaseResult<AuthenticationResponse>> AuthenticateByJwtTokenOfGoogleType2(string authorizationHeader)
        {
        //this is not working but need this properly to make standard also if this fixed then all will be fixed
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, "auth header is missed man"));
            }

        string token = authorizationHeader.Substring("Bearer ".Length);

        // Here, the token is read from the authorization header

        try
            {
            var tokenValidationParameters = new TokenValidationParameters
                {
                ValidIssuer = configuration["Google:Issuer"], //jwtSettings.Issuer,
                ValidAudience = configuration["Google:ClientId"],//app client id
                ValidateIssuerSigningKey = false,
                //ValidateSignatureLast=false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                    {
                        var jwt = new JwtSecurityToken(token);

                        return jwt;
                        },
                //     IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("ac3e3e558111c7c7a75c5b65134d22f63ee006d0"))
                //"ac3e3e558111c7c7a75c5b65134d22f63ee006d0"
                // ... other validation parameters (see previous explanation) ...
                };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var result = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            // ... (rest of your code for processing the validated token) ...
            }
        catch (SecurityTokenException ex)
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, "not mannnnn"));
            }
        return null;
        }

    }