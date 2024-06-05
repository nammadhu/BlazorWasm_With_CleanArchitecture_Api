using Microsoft.AspNetCore.Authorization.Infrastructure;
using PublicCommon;
using System.Security.Claims;

namespace BlazorWebApp.Shared
    {
    public class ClientConfig
        {//singleton
        public AppSettingsForClient? Settings { get; private set; }

        public bool ApiTokenFetching { get; private set; }
        public bool ApiTokenFetched { get; private set; }

        public string? Email { get; private set; }
        public Guid? UserId { get; private set; }
        public string? GToken { get; private set; }
        public string? AToken { get; private set; }

        public void LogOut()
            {
            Email = null; UserId = null; ApiTokenFetching = false; ApiTokenFetched = false;
            GToken = null; AToken = null; IsAdmin = null;
            }
        public ClientConfig SettingsUpdate(AppSettingsForClient newSettings)
            {//this executes when app started
            Settings = newSettings;
            return this;
            }

        public void ApiTokenFetchingUpdate(bool value = true) => ApiTokenFetching = value;

        public void EmailSet(string email) => Email = email;

        public bool? IsAdmin { get; private set; }
        public bool? IsInternalAdmin { get; private set; }
        public bool? IsInternalViewer { get; private set; }
        public bool? IsCardCreator { get; private set; }
        public bool? IsCardOwner { get; private set; }
        public bool? IsCardApprovedOwner { get; private set; }
        public bool? IsCardReviewer { get; private set; }
        public bool? IsBlocked { get; private set; }

        public void RolesSet(IEnumerable<Claim>? claims)
            {
            if (claims.HasData<Claim>())
                {
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
               
                    IsAdmin = roles?.Contains(CONSTANTS.Auth.Role_Admin)??false;
                    IsInternalAdmin = roles?.Contains(CONSTANTS.Auth.Role_InternalAdmin) ?? false;
                    IsInternalViewer = roles?.Contains(CONSTANTS.Auth.Role_InternalViewer) ?? false;
                    IsCardCreator = roles?.Contains(CONSTANTS.Auth.Role_CardCreator) ?? false;
                    IsCardOwner = roles?.Contains(CONSTANTS.Auth.Role_CardOwner) ?? false;
                    IsCardReviewer = roles?.Contains(CONSTANTS.Auth.Role_CardApprovedReviewer) ?? false;
                    IsCardApprovedOwner = roles?.Contains(CONSTANTS.Auth.Role_CardApprovedOwner) ?? false;
                    IsBlocked = roles?.Contains(CONSTANTS.Auth.Role_Blocked) ?? false;
                }
            else
                {
                //not logged in so dont set,let it be be null as it is
                }
            }

        public void UserIdSet(string userId) => UserId = Guid.TryParse(userId, out Guid result) ? result : Guid.Empty;
        public void UserIdSet(Guid userId) => UserId = userId;
        public void ApiTokenFetchedUpdate(bool value = true)
            {
            ApiTokenFetched = value;
            if (value == true)
                ApiTokenFetching = false;
            }

        }
    }
