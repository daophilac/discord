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
    [Route("api/Server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly MainDatabase context;

        public ServerController(MainDatabase context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Server>>> GetByUser(int userId) {
            IQueryable<ServerUser> tempListServerUser = context.ServerUser.Where(su => su.UserId == userId).AsQueryable();
            List<Server> listServer = await context.Server.Where(s => tempListServerUser.Any(su => su.ServerId == s.ServerId)).Select(server => Server.Clone(server)).ToListAsync();
            foreach (Server server in listServer) {
                User admin = await context.User.Where(u => u.UserId == server.AdminId).FirstOrDefaultAsync();
                List<ServerUser> listServerUser = await context.ServerUser.Where(su => su.ServerId == server.ServerId).ToListAsync();
                server.Admin = admin;
                server.ServerUsers = listServerUser;
                foreach (ServerUser serverUser in server.ServerUsers) {
                    User user = await context.User.Where(u => u.UserId == serverUser.UserId).FirstOrDefaultAsync();
                    serverUser.User = user;
                }
            }
            return Ok(listServer);
        }

        [HttpPost]
        [Route("Add")]
        public ActionResult<Server> Add(Server serverFromClient) {
            context.Server.Add(serverFromClient);
            context.SaveChanges();
            serverFromClient = context.Server.Last();
            ServerUser serverUser = new ServerUser();
            serverUser.ServerId = serverFromClient.ServerId;
            serverUser.UserId = serverFromClient.AdminId;
            context.ServerUser.Add(serverUser);
            context.SaveChanges();
            InstantInvite instantInvite = new InstantInvite();

            //TODO
            instantInvite.ServerId = serverFromClient.ServerId;
            instantInvite.Link = Guid.NewGuid().ToString();
            instantInvite.NerverExpired = true;
            context.InstantInvite.Add(instantInvite);
            context.SaveChanges();

            return Ok(serverFromClient);
        }








       // GET: api/Server
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServer() {
            return await context.Server.ToListAsync();
        }

        // GET: api/Server/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServer(int id)
        {
            var server = await context.Server.FindAsync(id);

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

            context.Entry(server).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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
            context.Server.Add(server);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetServer", new { id = server.ServerId }, server);
        }

        // DELETE: api/Server/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Server>> DeleteServer(int id)
        {
            var server = await context.Server.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            context.Server.Remove(server);
            await context.SaveChangesAsync();

            return server;
        }

        private bool ServerExists(int id)
        {
            return context.Server.Any(e => e.ServerId == id);
        }
    }
}
