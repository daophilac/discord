using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/ChannelPermission")]
    [ApiController]
    public class ChannelPermissionController : ControllerBase
    {
        private readonly MainDatabase _context;

        public ChannelPermissionController(MainDatabase context)
        {
            _context = context;
        }
        [HttpGet, Route("Get/{channelId}/{roleId}")]
        public async Task<IActionResult> Get(int channelId, int roleId) {
            return Ok(await _context.ChannelPermission.Where(cp => cp.ChannelId == channelId && cp.RoleId == roleId).FirstOrDefaultAsync());
        }

        // GET: api/ChannelPermission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChannelPermission>>> GetChannelPermission()
        {
            return await _context.ChannelPermission.ToListAsync();
        }

        // GET: api/ChannelPermission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelPermission>> GetChannelPermission(int id)
        {
            var channelPermission = await _context.ChannelPermission.FindAsync(id);

            if (channelPermission == null)
            {
                return NotFound();
            }

            return channelPermission;
        }

        // PUT: api/ChannelPermission/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannelPermission(int id, ChannelPermission channelPermission)
        {
            if (id != channelPermission.ChannelId)
            {
                return BadRequest();
            }

            _context.Entry(channelPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelPermissionExists(id))
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

        // POST: api/ChannelPermission
        [HttpPost]
        public async Task<ActionResult<ChannelPermission>> PostChannelPermission(ChannelPermission channelPermission)
        {
            _context.ChannelPermission.Add(channelPermission);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChannelPermissionExists(channelPermission.ChannelId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChannelPermission", new { id = channelPermission.ChannelId }, channelPermission);
        }

        // DELETE: api/ChannelPermission/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChannelPermission>> DeleteChannelPermission(int id)
        {
            var channelPermission = await _context.ChannelPermission.FindAsync(id);
            if (channelPermission == null)
            {
                return NotFound();
            }

            _context.ChannelPermission.Remove(channelPermission);
            await _context.SaveChangesAsync();

            return channelPermission;
        }

        private bool ChannelPermissionExists(int id)
        {
            return _context.ChannelPermission.Any(e => e.ChannelId == id);
        }
    }
}
