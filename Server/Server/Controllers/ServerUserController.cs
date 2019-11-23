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
    [Route("api/ServerUser")]
    [ApiController]
    public class ServerUserController : ControllerBase
    {
        private readonly MainDatabase _context;

        public ServerUserController(MainDatabase context)
        {
            _context = context;
        }

        [HttpDelete, Route("LeaveServer/{userId}/{serverId}")]
        public ActionResult LeaveServer(int userId, int serverId) {
            ServerUser serverUser = _context.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).First();
            _context.ServerUser.Remove(serverUser);
            _context.SaveChanges();
            return Ok();
        }



        // GET: api/ServerUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerUser>>> GetServerUser()
        {
            return await _context.ServerUser.ToListAsync();
        }

        // GET: api/ServerUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerUser>> GetServerUser(int id)
        {
            var serverUser = await _context.ServerUser.FindAsync(id);

            if (serverUser == null)
            {
                return NotFound();
            }

            return serverUser;
        }

        // PUT: api/ServerUser/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServerUser(int id, ServerUser serverUser)
        {
            if (id != serverUser.ServerId)
            {
                return BadRequest();
            }

            _context.Entry(serverUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerUserExists(id))
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

        // POST: api/ServerUser
        [HttpPost]
        public async Task<ActionResult<ServerUser>> PostServerUser(ServerUser serverUser)
        {
            _context.ServerUser.Add(serverUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServerUserExists(serverUser.ServerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServerUser", new { id = serverUser.ServerId }, serverUser);
        }

        // DELETE: api/ServerUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServerUser>> DeleteServerUser(int id)
        {
            var serverUser = await _context.ServerUser.FindAsync(id);
            if (serverUser == null)
            {
                return NotFound();
            }

            _context.ServerUser.Remove(serverUser);
            await _context.SaveChangesAsync();

            return serverUser;
        }

        private bool ServerUserExists(int id)
        {
            return _context.ServerUser.Any(e => e.ServerId == id);
        }
    }
}
