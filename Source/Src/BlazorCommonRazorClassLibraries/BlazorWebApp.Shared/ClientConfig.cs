using PublicCommon;

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
        public bool? IsAdmin { get; private set; }

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
        public void IsAdminSet(bool isAdmin) => IsAdmin = isAdmin;
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
