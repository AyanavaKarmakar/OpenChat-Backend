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
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<h1 align=\"center\">Welcome to OpenChat API!</h1>",
                StatusCode = 200
            };
        }
    }
}