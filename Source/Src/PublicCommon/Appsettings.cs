using AutoMapper;
using Microsoft.Extensions.Configuration;
using static PublicCommon.CONSTANTS;

namespace PublicCommon
    {

    public class AppConfigurations
        {
        public AppSettings? AppSettings { get; private set; }

        /// <summary>
        /// this should not have any sensitive personal information
        /// </summary>
        public AppSettingsForClient? AppSettingsForClient { get; private set; }
        public string EnvironmentName { get; private set; }
        public void SetEnvironment(EnvironmentEnum? nameSetForce = null)
            {
            if (nameSetForce == null)
                {
                if (System.Diagnostics.Debugger.IsAttached)
                    EnvironmentName = EnvironmentConsts.Name ?? EnvironmentConsts.Development;
                else
                    //above IfElse  is only to handle local deployment switch handling
                    EnvironmentName = EnvironmentConsts.Production;
                }
            else { EnvironmentName = Enum.GetName(typeof(EnvironmentEnum), nameSetForce); }
            Environment.SetEnvironmentVariable(EnvironmentConsts.ASPNETCORE_ENVIRONMENT, EnvironmentName);
            }
        public void Initialize(IConfiguration configuration)
            {
            //common settings loaded here
            AppSettings = configuration.Get<AppSettings>();
            if (AppSettings != null)
                {
                if (!string.IsNullOrEmpty(configuration["BUILDNUMBER"]))
                    AppSettings.BuildNumber = configuration["BUILDNUMBER"]!;//this is fetched from az pipleine
                else
                    AppSettings.BuildNumber = "NoBuildNo:" + DateTime.Now.ToString();
                //configured using azcli command below & it gets filled at runtime
                //az webapp config appsettings set --name nextmp --resource-group rg-Next --settings BUILDNUMBER=$BUILD_BUILDID

                var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AppSettingsToClientSettingsProfile>()).CreateMapper();
                AppSettingsForClient = mapper.Map<AppSettingsForClient>(AppSettings);

                AppSettingsForClient.JWTSettings = null;
                AppSettingsForClient.ConnectionStrings = null;
                foreach (var item in AppSettings.Authentications)
                    {
                    item.ClientSecret = null;
                    }
                }

            //var connectionString = _configuration.GetConnectionString("BlobStorage");
            }
        public AppSettingsForClient GetAppSettingsForClient(string? ip)
            {
            if (ip != null)
                AppSettingsForClient.IpAddressClientUser = ip;
            AppSettingsForClient.LoadedDate = DateTime.Now;
            return AppSettingsForClient;
            }
        }

    public class AppSettingsForClient : AppSettings
        {
        public DateTime LoadedDate { get; set; } = DateTime.Now;
        }
    public class AppSettingsToClientSettingsProfile : Profile
        {
        public AppSettingsToClientSettingsProfile()
            {
            //we can do here like AppSettingsForClient.ConnectionStrings = null;
            CreateMap<AppSettings, AppSettingsForClient>();
            }
        }
    public class AppSettings : AppSettingsBase
        {
        public string BuildNumber { get; set; } = DateTime.Now.ToString();
        //here environment specific settings will be added
        public required ConnectionStrings ConnectionStrings { get; set; }
        //if needed add authentication also
        public required JWTSettings JWTSettings { get; set; }
        }
    public class AppSettingsBase
        {
        public string EnvironmentName { get; set; }
        public string CompanyName { get; set; } = "Katthe Softwares & Solutions, India";
        public string CompanTagLine { get; set; } = "Software & Solutions with a Cause";
        public string CompanyUrl { get; set; } = "https://www.Katthe.com";

        public string ContactEmail { get; set; }


        public string PublicDomain { get; set; }//"next-mp.in"
        public string PublicDomainAbsoluteUrl { get; set; }//"https://www.next-mp.in" this is with https://www. so on display had to remove & use
        public string PublicDomainAbsoluteUrlSecond { get; set; }//"https://www.MP24.in" this is with https://www. so on display had to remove & use

        public string Title { get; set; } = "Katthe Softwares & Solutions with a Cause";//"MP DareDevil Transparent Secure Feedback Voting System & Survey"
        public string? FaviconImage { get; set; } = "_content/BaseBlazorComponentsRCL/images/faviconKATTHELogo.png";
        public string Description { get; set; }//"Know about your Member of Paliament MP and share current situation of your Constituency Problems Corruption Dictatorship Illegal Actions Steps Corrections"
        public string AppVideoUrl { get; set; }//"https://youtu.be/Ktc8GLW3QZo"
        public List<AuthenticationConfigurations> Authentications { get; set; }
        //public string BaseUri { get; set; }//this will not be used ,instead navigationmanager is taking 
        //public bool DetailedErrors { get; set; }
        public LoggingSettings Logging { get; set; }
        public string AllowedHosts { get; set; }
        public bool DetailedErrors { get; set; }
        public string IpAddressClientUser { get; set; }
        public VotingSystem VotingSystem { get; set; }
        }


    public class AuthenticationConfigurations
        {
        public string Type { get; set; }//google,facebook
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


        public string Authority { get; set; }
        public string ValidIssuer { get; set; }

        public string PostLogoutRedirectUri { get; set; }
        public string RedirectUri { get; set; }

        public string ResponseType { get; set; }

        public string[] Claims { get; set; }

        //   "Authority": "https://accounts.google.com/",
        //"ValidIssuer": "https://accounts.google.com/",
        //"PostLogoutRedirectUri": "https://localhost:7195/authentication/logout-callback",
        //"RedirectUri": "https://localhost:7195/authentication/login-callback",
        //"ResponseType": "id_token"

        }


#pragma warning disable
    public class JWTSettings
        {
        /* "JWTSettings": {
    "Key": "C1CF4B7DC4C4175B6618DE4F55CA4AAA",
    "Issuer": "CoreIdentity",
    "Audience": "CoreIdentityUser",
    "DurationInMinutes": 15
  },*/
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
        }
#pragma warning disable 
    public class ConnectionStrings
        {
        public string DatabaseConnection { get; set; }
        public string BlobStorage { get; set; }
        }

    public class LoggingSettings
        {
        public Dictionary<string, string> LogLevel { get; set; }
        }
    public class VotingSystem
        {
        public string SystemType { get; set; }//MP
        public string CandidateType { get; set; }//MP
                                                 // public string PublicDomainUrl { get; set; }//Next-Mp.in //moved to top level

        }

    }
