namespace PublicCommon
    {
    //cachekey refreshinterval set inside VoteCacheKey class refreshInterval

    public enum ApplicationTypeEnum
        {
        Katthe = 0,
        MyVote = 1,
        MyTown = 2,
        MyProducts = 3
        }

    public enum EnvironmentEnum
        {
        Development,
        Test,
        Production
        }
    public static class CONSTANTS
        {

        public const string ClientAnonymous = "AnonymousClient";
        public const string ClientAuthorized = "AuthClient";


        public const string ClientConfigurations = "ClientConfigurations";
        public const string MyTownApp = "MyTown";
        public const string MyTownAppKey = "mYtOWN";
        public const string MyTownAppKeyAuth = "mYtOWNsECURE";
        public const string AppKeyName = "X-Encrypted-Content";

        public const string Bearer = "Bearer";
        public const string Authorization = "Authorization";
        public const string ApplicationJson = "application/json";

        public const string Email = "Email";
        public const string UserId = "UserId";

        public static class EnvironmentConsts
            {

            public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
            public static readonly string Name = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);
            public const string Development = nameof(EnvironmentEnum.Development);
            public const string Production = nameof(EnvironmentEnum.Production);
            public const string Test = nameof(EnvironmentEnum.Test);
            public static readonly bool IsDevelopment = (Name == Development);
            public static readonly bool IsProduction = (Name == Production);
            }
        public static class Auth
            {
            public const string Role_Admin = "Admin";
            public const string Role_InternalAdmin = "InternalAdmin";
            public const string Role_InternalViewer = "InternalViewer";
            /// <summary>
            /// once card created then he becomes Creator 
            /// </summary>
            public const string Role_CardCreator = "CardCreator";

            /// <summary>
            /// CardOwner by default when he created, otherwise on transferring of card
            /// </summary>
            public const string Role_CardOwner = "CardOwner";

            /// <summary>
            /// when any card approved he gets this (ApprovedCard table has Ownerid)
            /// </summary>
            public const string Role_CardApprovedOwner = "CardApprovedOwner";

            /// <summary>
            /// when someone added as reviewer or reviewed by himself
            /// </summary>
            public const string Role_CardApprovedReviewer = "CardReviewer";
            
            
            //Town main page,option works with Owner role only,if owner then only he can edit,not with creator

            //Any AuthenticatedUser //no separate role required
            //Anonymous //no separate role required

            public const string Role_Blocked = "Blocked";
            public static class ExternalProviders
                {
                public const string Google = "Google";
                public const string Facebook = "Facebook";
                public const string Twitter = "Twitter";
                public const string Microsoft = "Microsoft";
                }
            }

        }
    }
