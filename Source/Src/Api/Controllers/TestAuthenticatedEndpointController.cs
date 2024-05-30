namespace CleanArchitecture.WebApi.Controllers
    {
    [Authorize]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TestAuthenticatedEndpointController() : ControllerBase
        {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        public string Get()
            {
            //make sure of return type OK(string) & "string" both different
            //currently in code tried to handle both in localstorage extensions

            /* this is for sake of extra validations of token learning
            if (Request != null && Request?.Headers != null && !Request.Headers.Authorization.IsNullOrEmpty())
                {
                var token = Request.Headers.Authorization.ToString().Split(' ')[1];//bcz "Bearer asdjadsjlassdohujiqwerljwerjlitokenStaysHere"
                //Console.WriteLine($"[{token}]");

                //type1
                var type1Check = accountServices.AuthenticateWithGoogle(token).Result;

                //type2
                var type2Check = accountServices.AuthenticateByJwtTokenOfGoogleType2(Request.Headers.Authorization).Result;//this is working with some tweak of key

                return Ok(type1Check);
                //await
                }*/
            return "validated success";
            }
        [HttpPost]
        public string Post([FromBody] string value)
            {
            return value + value + DateTime.Now.ToString();
            }
        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //    {
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //        {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //        })
        //    .ToArray();
        //    }
        }
    }
