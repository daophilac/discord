using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Hubs;

namespace API.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MainDatabase _context;
        public MessageController(MainDatabase context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getmessagesbychannel/{channelID}")]
        public IQueryable<Message> GetMessagesByChannel(int channelID) {
            var messages = _context.Message.Where(m => m.ChannelId == channelID).OrderBy(m => m.Time);
            foreach (var message in messages) {
                _context.Entry(message).Reference(m => m.User).Load();
            }
            return messages;
        }


        [HttpPost]
        [Route("insertmessage")]
        public ActionResult<Message> InsertMessage(Message messageFromClient) {
            _context.Message.Add(messageFromClient);
            _context.SaveChanges();
            return Ok(messageFromClient);
        }
        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessage() {
            return await _context.Message.ToListAsync();
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id) {
            var message = await _context.Message.FindAsync(id);

            if (message == null) {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message) {
            if (id != message.MessageId) {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!MessageExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Message
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message) {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(int id) {
            var message = await _context.Message.FindAsync(id);
            if (message == null) {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return message;
        }

        private bool MessageExists(int id) {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
