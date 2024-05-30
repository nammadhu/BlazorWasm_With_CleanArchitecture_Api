using PublicCommon;

namespace BlazorWebApp.Shared.Services
    {
    public static class AuthHeader
        {
        public static readonly string Key = CONSTANTS.AppKeyName;
        public static readonly string Value = CONSTANTS.MyTownAppKeyAuth;
        }

    public static class AnonymousHeader
        {
        public static readonly string Key = CONSTANTS.AppKeyName;
        public static readonly string Value = CONSTANTS.MyTownAppKey;
        }
    }
