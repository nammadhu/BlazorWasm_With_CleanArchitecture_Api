namespace SharedResponse
    {
    public class ApiEndPoints
        {

        public const string Version = "V1";

        public const string ClientAnonymous = "AnonymousClient";
        public const string ClientAuth = "AuthClient";

        public const string Prefix = "api";

        public const string Town = "Town";
        public const string TownCard = "TownCard";
        public const string TownCardType = "TownCardType";

        public static string BaseUrl(string type)
            {
            return $"{Version}/{type}";
            }

        public const string Config = "Config";
        public const string Vote = "Vote";
        public const string Constituency = "Constituency";
        public const string VoteSupportOppose = "VoteSupportOppose";

        //public const string VoteGetWithMyUserId = $"{Prefix}/Vote";
        //public const string VotePost = $"{Prefix}/Vote";

        //public const string ConstituencyGetAll = $"{Prefix}/Constituency";

        //public const string VoteSupportOpposePost = $"{Prefix}/VoteSupportOppose";

        //[("api/v{version:apiVersion}/[controller]/[action]")]
        public const string ConfigExtractionGet = "v1/Config/Get";
        //public const string TokenExtractionPost = "v1/AuthenticationConfigurations/Validate";
        public const string TokenExtractionPostGoogle = "v1/Auth/ValidateG";
        public const string ApiIssuedJwt = "ApiIssuedJwt";

        public const string GetAll = "GetAll";
        public const string GetById = "GetById";
        public const string GetPagedList = "GetPagedList";
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";


        }
    }
