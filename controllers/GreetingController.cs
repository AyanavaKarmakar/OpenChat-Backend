using Microsoft.AspNetCore.Mvc;

namespace OpenChat.Greeting.Controllers
{
    [ApiController]
    [Route("/")]
    public class GreetingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                message = "Welcome to OpenChat!"
            };
            return Ok(response);
        }
    }
}