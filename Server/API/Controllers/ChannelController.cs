using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly MainDatabase _context;

        public ChannelController(MainDatabase context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getbyserver/{serverID}")]
        public IQueryable<Channel> GetByServer(int serverID) {
            var channels = from channel in _context.Channel where channel.ServerId == serverID select channel;
            return channels;
        }





        // GET: api/Channel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetChannel()
        {
            return await _context.Channel.ToListAsync();
        }

        // GET: api/Channel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Channel>> GetChannel(int id)
        {
            var channel = await _context.Channel.FindAsync(id);

            if (channel == null)
            {
                return NotFound();
            }

            return channel;
        }

        // PUT: api/Channel/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannel(int id, Channel channel)
        {
            if (id != channel.ChannelId)
            {
                return BadRequest();
            }

            _context.Entry(channel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Channel
        [HttpPost]
        public async Task<ActionResult<Channel>> PostChannel(Channel channel)
        {
            _context.Channel.Add(channel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChannel", new { id = channel.ChannelId }, channel);
        }

        // DELETE: api/Channel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Channel>> DeleteChannel(int id)
        {
            var channel = await _context.Channel.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            _context.Channel.Remove(channel);
            await _context.SaveChangesAsync();

            return channel;
        }

        private bool ChannelExists(int id)
        {
            return _context.Channel.Any(e => e.ChannelId == id);
        }
    }
}
