using CleanArchitecture.Application.DTOs.Account.Requests;
using Google.Apis.Auth;
using PublicCommon;
using SharedResponse.Wrappers;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.UserInterfaces;

public interface IAccountServices
{
    Task<BaseResult<AuthenticationResponse>> GetJwtByCreateAccountOrFetchWithSocialAsync(string userName, string email, string name, string subject, string loginProvider = CONSTANTS.Auth.ExternalProviders.Google);
    //Task<BaseResult<AuthenticationResponse>> AuthenticateWithGoogle(string googleJwtToken);
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleJwtToken(string googleJwtToken);

    Task<BaseResult<AuthenticationResponse>> AuthenticateByJwtTokenOfGoogleType2(string authorizationHeader);//working


    /* old for local auth
   
    Task<BaseResult<string>> RegisterGostAccount();
    Task<BaseResult> ChangePassword(ChangePasswordRequest model);
    Task<BaseResult> ChangeUserName(ChangeUserNameRequest model);
    Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request);
    Task<BaseResult<AuthenticationResponse>> AuthenticateByUserName(string username);
    */
}
