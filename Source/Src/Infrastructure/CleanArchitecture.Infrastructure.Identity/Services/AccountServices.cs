using CleanArchitecture.Application.DTOs.Account.Requests;
using CleanArchitecture.Application.Helpers;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Interfaces.UserInterfaces;
using CleanArchitecture.Infrastructure.Identity.Contexts;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PublicCommon;
using SharedResponse;
using SharedResponse.Wrappers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Services
    {
    public class AccountServices(UserManager<ApplicationUser> userManager, IAuthenticatedUserService authenticatedUser, SignInManager<ApplicationUser> signInManager, JWTSettings jwtSettings, ITranslator translator, IConfiguration configuration, AppConfigurations appConfigurations) : IAccountServices
        {

        public async Task<IdentityResult> AddCurrentUserToRole(string role)
            {
            if (!string.IsNullOrEmpty(authenticatedUser?.UserId))
                {
                var user = await userManager.FindByIdAsync(authenticatedUser.UserId);
                if (user != null)
                    return await userManager.AddToRoleAsync(user, role);
                }
            return new IdentityResult();
            }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
            {
            await userManager.UpdateSecurityStampAsync(user);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                 audience: jwtSettings.Audience,
                claims: await GetClaimsAsync(),
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;

            async Task<IList<Claim>> GetClaimsAsync()
                {
                var userClaims = await signInManager.ClaimsFactory.CreateAsync(user);
                //if any issue in roles extraction then below separate
                //var allClaimsINcludingRole = userClaims.Claims.ToList();
                //foreach (var role in user.Roles) //await userManager.GetRolesAsync(user))
                //    {
                //    var newClaim = new Claim(ClaimTypes.Role, role);
                //    if (!allClaimsINcludingRole.Exists(x => x.Type == newClaim.Type && x.Value == newClaim.Value))
                //        allClaimsINcludingRole.Add(newClaim);
                //    }
                return userClaims.Claims.ToList();
                }
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


        #region NotUsingFeatures

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
        //dont use
        public async Task<BaseResult> ChangePassword(ChangePasswordRequest model)
            {
            var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var identityResult = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (identityResult.Succeeded)
                return new BaseResult();

            return new BaseResult(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));
            }

        //dont use
        public async Task<BaseResult> ChangeUserName(ChangeUserNameRequest model)
            {
            var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

            user.UserName = model.UserName;

            var identityResult = await userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
                return new BaseResult();

            return new BaseResult(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));
            }

        //dont use
        private async Task<BaseResult<AuthenticationResponse>> AuthenticateDontUseOnlyReference(AuthenticationRequest request)
            {
            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
                {
                return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_notfound_with_UserName(request.UserName)), nameof(request.UserName)));
                }
            var result = await signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                {
                return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.FieldDataInvalid, translator.GetString(TranslatorMessages.AccountMessages.Invalid_password()), nameof(request.Password)));
                }

            var rolesList = await userManager.GetRolesAsync(user);

            var jwToken = await GenerateJwtToken(user);

            AuthenticationResponse response = new AuthenticationResponse()
                {
                Id = user.Id.ToString(),
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = [.. rolesList],
                IsVerified = user.EmailConfirmed,
                };

            return new BaseResult<AuthenticationResponse>(response);
            }

        //dont use
        public async Task<BaseResult<AuthenticationResponse>> AuthenticateByUserName(string username)
            {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                {
                return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_notfound_with_UserName(username)), nameof(username)));
                }

            var rolesList = await userManager.GetRolesAsync(user);

            var jwToken = await GenerateJwtToken(user);

            AuthenticationResponse response = new AuthenticationResponse()
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

        //dont use
        public async Task<BaseResult<string>> RegisterGostAccount()
            {
            var user = new ApplicationUser()
                {
                UserName = GenerateRandomString(7)
                };

            var identityResult = await userManager.CreateAsync(user);

            if (identityResult.Succeeded)
                return new BaseResult<string>(user.UserName);

            return new BaseResult<string>(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));

            string GenerateRandomString(int length)
                {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                return new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                }
            }
        #endregion NotUsingFeatures
        }
    }