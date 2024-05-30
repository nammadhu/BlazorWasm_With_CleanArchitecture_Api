//using Microsoft.AspNetCore.Components;//dont add this it makes conflict with mvc 
using PublicCommon;

namespace CleanArchitecture.WebApi.Controllers.v1
    {
    //[Route($"{ApiEndPoints.Prefix}/{ApiEndPoints.Config}")]
    //[Route("api/[controller]")]
    //[ApiController]
    //[AllowAnonymous]
    [ApiVersion("1")]
    public class ConfigController(AppConfigurations appConfigurations) : BaseApiController
        {
        //Microsoft.AspNetCore.Components.NavigationManager _navigationManager;
        //public ConfigController(Microsoft.AspNetCore.Components.NavigationManager navigationManager)
        //    {
        //    _navigationManager = navigationManager;
        //    }

        [HttpGet]
        public IActionResult Get()
            {
            //string sasToken = string.Empty;
            //var domainName = Request.Headers.Host.FirstOrDefault();
            //lets disabled SAS token logic as of now,this is working but client side image getting authentication error so using directly blob url anonymous
            //if (domainName == "localhost:7216" ||
            //    domainName == "bhadravathi.com" || domainName == "bdvt.in")
            //    {
            //    sasToken = AzConnect.GenerateContainerReadSasTokenForDurationHr(ConfigurationProvider.AppSettings.ConnectionStrings.BlobStorage, "bhadravathi.com");
            //    }

            /*
        https://mytown.blob.core.windows.net/files-of-town/bhadravathi.com/Iron_City.jpg
        https://mytown.blob.core.windows.net/files-of-town/bhadravathi.com/visl.jpg

            sas token way
        https://mytown.blob.core.windows.net/bhadravathi.com?sv=2018-03-28&sr=c&sig=pFTpyKzpcbFI5wgai58E71F9gzefao73nRoi4LdXkF4%3D&se=2024-04-11T01%3A58%3A56Z&sp=rl 
                */

            return Ok(appConfigurations.GetAppSettingsForClient(HttpContext.Connection.RemoteIpAddress.ToString()));

            //return Ok(new ConfigurationsForWasmClient()
            //    {
            //    ContactEmail = appConfigurations.AppSettings.ContactEmail,
            //    PublicDomainAbsoluteUrl = appConfigurations.AppSettings.PublicDomainAbsoluteUrl,
            //    PublicDomainAbsoluteUrlSecond = appConfigurations.AppSettings.PublicDomainAbsoluteUrlSecond,
            //    IpAddressClientUser = HttpContext.Connection.RemoteIpAddress?.ToString(),
            //    //SasToken = sasToken,

            //    CandidateType = appConfigurations.AppSettings.VotingSystem?.CandidateType,
            //    SystemType = appConfigurations.AppSettings.VotingSystem?.SystemType,

            //    });
            }
        }
    }
