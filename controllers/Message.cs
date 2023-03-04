using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Chat.Models;

namespace OpenChat.Message.Controllers
{
    using Message = Chat.Models.Message;

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
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _db.Messages.ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _db.Messages.FirstOrDefaultAsync(message => message.Id == id);

            if (message == null)
            {
                return NotFound("Message not found");
            }

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            if (message == null)
            {
                return BadRequest("Message object is null");
            }

            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();

            return Ok(message);
        }
    }
}