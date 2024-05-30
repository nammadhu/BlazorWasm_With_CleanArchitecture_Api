// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.WebApi.Controllers
    {
    //[Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestAnonymousEndpointController : ControllerBase
        {
        // GET: api/<ValuesController>
        [HttpGet]
        public string Get()
            {
            return "value1madhu anonymous";
            }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
            {
            return "value " + id;
            }

        // POST api/<ValuesController>
        [HttpPost]
        public string Post([FromBody] string value)
            {
            //if (value != "madhu") throw new Exception("remove this line exception code");

            return value + DateTime.Now.ToString();
            }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
            {
            }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
            {
            }
        }
    }
