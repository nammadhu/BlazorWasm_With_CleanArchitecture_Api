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

//common
public partial class AccountServices
    {
    
    private async Task<AuthenticationResponse> GetAuthenticationResponse(ApplicationUser user)
        {
        var jwToken = await GenerateJwtToken(user);

        var rolesList = await userManager.GetRolesAsync(user);

        return new AuthenticationResponse()
            {
            Id = user.Id.ToString(),
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
            Email = user.Email,
            UserName = user.UserName,
            Roles = rolesList,
            IsVerified = user.EmailConfirmed,
            };
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
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;

        async Task<IList<Claim>> GetClaimsAsync()
            {
            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            return result.Claims.ToList();
            }
        }

   }