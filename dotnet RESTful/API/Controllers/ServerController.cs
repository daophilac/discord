using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    public class ServerController : ApiController
    {
        private APIContext db = new APIContext();

        // GET: api/Server
        public IQueryable<Server> GetServer()
        {
            return db.Server;
        }

        // GET: api/Server/5
        [ResponseType(typeof(Server))]
        public IHttpActionResult GetServer(int id)
        {
            Server server = db.Server.Find(id);
            if (server == null)
            {
                return NotFound();
            }

            return Ok(server);
        }

        // PUT: api/Server/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServer(int id, Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != server.ServerID)
            {
                return BadRequest();
            }

            db.Entry(server).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Server
        [ResponseType(typeof(Server))]
        public IHttpActionResult PostServer(Server server)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Server.Add(server);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = server.ServerID }, server);
        }

        // DELETE: api/Server/5
        [ResponseType(typeof(Server))]
        public IHttpActionResult DeleteServer(int id)
        {
            Server server = db.Server.Find(id);
            if (server == null)
            {
                return NotFound();
            }

            db.Server.Remove(server);
            db.SaveChanges();

            return Ok(server);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServerExists(int id)
        {
            return db.Server.Count(e => e.ServerID == id) > 0;
        }
    }
}