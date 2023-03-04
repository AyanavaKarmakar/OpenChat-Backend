using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Chat.Models;

namespace OpenChat.Message.Controllers
{
    [ApiController]
    [Route("/api/v1/messages")]
    public class MessageController : ControllerBase
    {
        private readonly ChatDB _db;

        public MessageController(ChatDB db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GettMessages()
        {
            var messages = await _db.Messages.ToListAsync();
            return Ok(messages);
        }
    }
}