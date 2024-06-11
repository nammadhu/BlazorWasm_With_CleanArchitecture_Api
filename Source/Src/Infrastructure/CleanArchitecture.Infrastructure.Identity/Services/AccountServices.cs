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
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Identity.Services;


public partial class AccountServices(UserManager<ApplicationUser> userManager, IAuthenticatedUserService authenticatedUser, SignInManager<ApplicationUser> signInManager, JWTSettings jwtSettings, ITranslator translator, IConfiguration configuration, AppConfigurations appConfigurations) : IAccountServices
    {
    public async Task<BaseResult> ChangePassword(ChangePasswordRequest model)
        {
        var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var identityResult = await userManager.ResetPasswordAsync(user, token, model.Password);

        if (identityResult.Succeeded)
            return new BaseResult();

        return new BaseResult(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));
        }

    public async Task<BaseResult> ChangeUserName(ChangeUserNameRequest model)
        {
        var user = await userManager.FindByIdAsync(authenticatedUser.UserId);

        user.UserName = model.UserName;

        var identityResult = await userManager.UpdateAsync(user);

        if (identityResult.Succeeded)
            return new BaseResult();

        return new BaseResult(identityResult.Errors.Select(p => new Error(ErrorCode.ErrorInIdentity, p.Description)));
        }

    public async Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null)
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_notfound_with_UserName(request.UserName)), nameof(request.UserName)));
            }

        var signInResult = await signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.FieldDataInvalid, translator.GetString(TranslatorMessages.AccountMessages.Invalid_password()), nameof(request.Password)));
            }

        var result = await GetAuthenticationResponse(user);

        return new BaseResult<AuthenticationResponse>(result);
        }

    public async Task<BaseResult<AuthenticationResponse>> AuthenticateByUserName(string username)
        {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
            {
            return new BaseResult<AuthenticationResponse>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.AccountMessages.Account_notfound_with_UserName(username)), nameof(username)));
            }

        var result = await GetAuthenticationResponse(user);

        return new BaseResult<AuthenticationResponse>(result);
        }

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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }