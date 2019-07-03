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
    [Route("api/instantinvite")]
    [ApiController]
    public class InstantInviteController : ControllerBase
    {
        private readonly MainDatabase _context;

        public InstantInviteController(MainDatabase context)
        {
            _context = context;
        }

        // GET: api/InstantInvite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstantInvite>>> GetInstantInvite()
        {
            return await _context.InstantInvite.ToListAsync();
        }

        [HttpGet]
        [Route("getserverbyinstantinvite/{id}/{link}")]
        public ActionResult<Server> getServerByLink(int id, string link) {
            InstantInvite instantInvite = _context.InstantInvite.Where(ii => ii.Link == link).FirstOrDefault();
            if(instantInvite != null) {
                Server result = _context.Server.Where(s => s.ServerId == instantInvite.ServerId).First();
                ServerUser serverUser = _context.ServerUser.Where(su => su.ServerId == result.ServerId && su.UserId == id).FirstOrDefault();
                if(serverUser == null) {
                    serverUser = new ServerUser();
                    serverUser.ServerId = result.ServerId;
                    serverUser.UserId = id;
                    _context.ServerUser.Add(serverUser);
                    _context.SaveChanges();
                }
                return Ok(result);
            }
            return NotFound();
        }

        // GET: api/InstantInvite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstantInvite>> GetInstantInvite(string id)
        {
            var instantInvite = await _context.InstantInvite.FindAsync(id);

            if (instantInvite == null)
            {
                return NotFound();
            }

            return instantInvite;
        }

        // PUT: api/InstantInvite/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstantInvite(string id, InstantInvite instantInvite)
        {
            if (id != instantInvite.Link)
            {
                return BadRequest();
            }

            _context.Entry(instantInvite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstantInviteExists(id))
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

        // POST: api/InstantInvite
        [HttpPost]
        public async Task<ActionResult<InstantInvite>> PostInstantInvite(InstantInvite instantInvite)
        {
            _context.InstantInvite.Add(instantInvite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstantInvite", new { id = instantInvite.Link }, instantInvite);
        }

        // DELETE: api/InstantInvite/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InstantInvite>> DeleteInstantInvite(string id)
        {
            var instantInvite = await _context.InstantInvite.FindAsync(id);
            if (instantInvite == null)
            {
                return NotFound();
            }

            _context.InstantInvite.Remove(instantInvite);
            await _context.SaveChangesAsync();

            return instantInvite;
        }

        private bool InstantInviteExists(string id)
        {
            return _context.InstantInvite.Any(e => e.Link == id);
        }
    }
}
