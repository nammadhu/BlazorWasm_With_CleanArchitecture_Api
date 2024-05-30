using CleanArchitecture.Application.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PublicCommon;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity
    {
    public class GoogleTokenAuthenticationOptions : AuthenticationSchemeOptions
        {
        }
    public class GoogleTokenAuthenticationHandler : AuthenticationHandler<GoogleTokenAuthenticationOptions>
        {
        IAccountServices _accountServices;
        public GoogleTokenAuthenticationHandler(
            IOptionsMonitor<GoogleTokenAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IAccountServices accountServices)
            : base(options, logger, encoder, clock)
            {
            _accountServices = accountServices;
            }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
            {
            // Extract the Google token from the Authorization header
            var googleToken = Request.Headers[CONSTANTS.Authorization].ToString().Replace(CONSTANTS.Bearer + " ", "");//not sure required or not
            var res = await _accountServices.ValidateGoogleJwtToken(googleToken);

            if (res != null)
                {
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, res.Name),
                        new(ClaimTypes.Email, res.Email),
                        new("Subject", res.Subject),
                        new("urn:google:profile", res.Picture),
                        new("Payload", JsonSerializer.Serialize(res))
                        //if anymore required from google token then extract here & assign
                    };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
                }
            else
                {
                Console.WriteLine("Invalid Google token");
                return AuthenticateResult.Fail("Invalid Google token");
                }
            }
        }
    }
