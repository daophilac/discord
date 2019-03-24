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
    public class ChannelController : ApiController
    {
        private APIContext db = new APIContext();


        [HttpGet]
        [Route("api/channel/getchannelsbyserver/{serverID}")]
        public IQueryable<Channel> GetChannelsByServer(int serverID) {
            var channels = from channel in db.Channel where channel.ServerID == serverID select channel;
            return channels;
        }
        // GET: api/Channel
        public IQueryable<Channel> GetChannel()
        {
            return db.Channel;
        }

        // GET: api/Channel/5
        [ResponseType(typeof(Channel))]
        public IHttpActionResult GetChannel(int id)
        {
            Channel channel = db.Channel.Find(id);
            if (channel == null)
            {
                return NotFound();
            }

            return Ok(channel);
        }

        // PUT: api/Channel/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChannel(int id, Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != channel.ChannelID)
            {
                return BadRequest();
            }

            db.Entry(channel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Channel
        [ResponseType(typeof(Channel))]
        public IHttpActionResult PostChannel(Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Channel.Add(channel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = channel.ChannelID }, channel);
        }

        // DELETE: api/Channel/5
        [ResponseType(typeof(Channel))]
        public IHttpActionResult DeleteChannel(int id)
        {
            Channel channel = db.Channel.Find(id);
            if (channel == null)
            {
                return NotFound();
            }

            db.Channel.Remove(channel);
            db.SaveChanges();

            return Ok(channel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChannelExists(int id)
        {
            return db.Channel.Count(e => e.ChannelID == id) > 0;
        }
    }
}