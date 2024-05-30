using CleanArchitecture.Application.Interfaces.UserInterfaces;
using Google.Apis.Auth;
using PublicCommon;
using System.Text.Json;
namespace CleanArchitecture.WebApi.Controllers.v1
    {

    //[Authorize]
    [ApiVersion("1")]
    public class AuthController(IAccountServices accountServices) : BaseApiController
        {
        /*
        [HttpPost]
        [AllowAnonymous]//this works but since its anonymous so avoiding & using other method with header valdiation
        public async Task<BaseResult<AuthenticationResponse>> Validate([FromBody] ValidateRequest request)
            {
            if (!string.IsNullOrEmpty(request.GoogleToken))
                {
                return await accountServices.AuthenticateWithGoogle(request.GoogleToken);
                }
            return new BaseResult<AuthenticationResponse>() { Success = false };
            }
        public class ValidateRequest
            {
            public string GoogleToken { get; set; }
            }
        */


        [HttpPost]
        [Authorize(AuthenticationSchemes = "GoogleToken")]
        public async Task<BaseResult<AuthenticationResponse>> ValidateG([FromBody] ValidateAppRequest request)
            {
            //if (!string.IsNullOrEmpty(request.AppKey))
            //    Console.WriteLine(request.AppKey);

            //Console.WriteLine("success1");
            //var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Console.WriteLine(userEmail);
            //var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            //var subject = User.FindFirst(ClaimTypes.Name)?.Value;

            var payloadAsString = User.FindFirst("Payload")?.Value;

            if (payloadAsString != null)
                {
                var payLoad = JsonSerializer.Deserialize<GoogleJsonWebSignature.Payload>(payloadAsString);

                return await accountServices.GetJwtByCreateAccountOrFetchWithSocialAsync(payLoad.Email, payLoad.Email, payLoad.Name, payLoad.Subject, CONSTANTS.Auth.ExternalProviders.Google);
                //Console.WriteLine("res came");
                //Console.WriteLine(res.Data.JWToken);

                }

            //if (!string.IsNullOrEmpty(request.GoogleToken))
            //    {
            //    return await accountServices.AuthenticateWithGoogle("");
            //    }
            return new BaseResult<AuthenticationResponse>() { Success = false };
            }
        }



    public class ValidateAppRequest
        {
        public string AppKey { get; set; }
        }
    }
