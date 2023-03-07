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
            var messages = await _db.Messages.OrderByDescending(message => message.Timestamp).ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _db.Messages.FirstOrDefaultAsync(message => message.Id == id);

            if (message == null)
            {
                var NotFoundResponse = new
                {
                    message = "Message not found"
                };
                return NotFound(NotFoundResponse);
            }

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            if (message == null)
            {
                var BadRequestResponse = new
                {
                    message = "Message object is null"
                };
                return BadRequest(BadRequestResponse);
            }

            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();

            return Ok(message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, Message message)
        {
            if (message == null)
            {
                var BadRequestResponse = new
                {
                    message = "Message object is null"
                };
                return BadRequest(BadRequestResponse);
            }

            var messageToUpdate = await _db.Messages.FirstOrDefaultAsync(message => message.Id == id);

            if (messageToUpdate == null)
            {
                var NotFoundResponse = new
                {
                    message = "Message not found"
                };
                return NotFound(NotFoundResponse);
            }

            messageToUpdate.MessageContent = message.MessageContent;
            messageToUpdate.Timestamp = message.Timestamp;

            await _db.SaveChangesAsync();

            return Ok(messageToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _db.Messages.FirstOrDefaultAsync(message => message.Id == id);

            if (message == null)
            {
                var BadRequestResponse = new
                {
                    message = "Message object is null"
                };
                return BadRequest(BadRequestResponse);
            }

            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}