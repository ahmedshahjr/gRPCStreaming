using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimeNumberController : ControllerBase
    {
        // GET: api/<PrimeNumberController>
        [HttpGet]
        public async Task<IActionResult> Post(CancellationToken cancellationToken)
        {
            return Ok(Ok());
          //  return new string[] { "value1", "value2" };
        }
    }
}
