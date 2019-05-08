using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.IO;

namespace API.Controllers
{
    [Route("api/server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly MainDatabase _context;

        public ServerController(MainDatabase context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getserversbyuser/{userID}")]
        public IQueryable<Server> GetServersByUser(int userID) {
            var servers = _context.Server.Where(s => _context.ServerUser.Where(su => su.UserId == userID).Any(su => su.ServerId == s.ServerId));
            return servers;
        }

        [HttpGet]
        [Route("getserverimage/{id}")]
        public async Task<IActionResult> GetServerImage(int id) {
            string path = "D:\\Desktop\\Images\\Server\\server_1.png";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open)) {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "image/png", Path.GetFileName(path));
        }

        [HttpPost]
        [Route("insertserver")]
        public ActionResult<Server> InsertServer(Server serverFromClient) {
            _context.Server.Add(serverFromClient);
            _context.SaveChanges();
            serverFromClient = _context.Server.Last();
            ServerUser serverUser = new ServerUser();
            serverUser.ServerId = serverFromClient.ServerId;
            serverUser.UserId = serverFromClient.AdminId;
            _context.ServerUser.Add(serverUser);
            _context.SaveChanges();
            return serverFromClient;
        }








       // GET: api/Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServer() {
            return await _context.Server.ToListAsync();
        }

        // GET: api/Server/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServer(int id)
        {
            var server = await _context.Server.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            return server;
        }

        // PUT: api/Server/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer(int id, Server server)
        {
            if (id != server.ServerId)
            {
                return BadRequest();
            }

            _context.Entry(server).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
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

        // POST: api/Server
        [HttpPost]
        public async Task<ActionResult<Server>> PostServer(Server server)
        {
            _context.Server.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServer", new { id = server.ServerId }, server);
        }

        // DELETE: api/Server/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Server>> DeleteServer(int id)
        {
            var server = await _context.Server.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            _context.Server.Remove(server);
            await _context.SaveChangesAsync();

            return server;
        }

        private bool ServerExists(int id)
        {
            return _context.Server.Any(e => e.ServerId == id);
        }
    }
}
