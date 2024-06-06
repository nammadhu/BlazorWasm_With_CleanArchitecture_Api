using CleanArchitecture.Application.DTOs.Account.Requests;
using CleanArchitecture.Application.DTOs.Account.Responses;
using SharedResponse.Wrappers;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.UserInterfaces;

public interface IGetUserServices
{
    Task<PagedResponse<UserDto>> GetPagedUsers(GetAllUsersRequest model);
}
