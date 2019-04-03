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
    public class UserController : ApiController
    {
        private APIContext db = new APIContext();
        [HttpGet]
        [Route("api/user/login2/{email}/{password}")]
        public IHttpActionResult Login2(string email, string password) {
            var user = from u in db.User where u.Email == email && u.Password == password select u;
            if (user == null) {
                return NotFound();
            }
            return Ok(user.First());
        }
        [HttpPost]
        [Route("api/user/login")]
        [ResponseType(typeof(User))]
        public IHttpActionResult Login(User userFromClient) {
            var user = from u in db.User where u.Email == userFromClient.Email && u.Password == userFromClient.Password select u;
            if (user == null) {
                return NotFound();
            }
            return Ok(user.First());
        }
        [HttpPost]
        [Route("api/user/login3")]
        [ResponseType(typeof(User))]
        public IHttpActionResult Login3(User userFromClient) {
            var user = from u in db.User where u.Email == userFromClient.Email && u.Password == userFromClient.Password select u;
            if (user == null) {
                return NotFound();
            }
            return Ok(user.First());
        }
        [HttpGet]
        [Route("api/user/taotaikhoan/{email}/{password}/{username}/{firstname}/{lastname}/{image}")]
        public void taotaikhoan(string email, string password, string username, string firstname, string lastname, string image) {
            User u = new User { Email = email, Password = password, UserName = username, FirstName = firstname, LastName = lastname, Gender = Gender.Female, Image = null };
            APIContext context = new APIContext();
            context.User.Add(u);
            context.SaveChanges();
        }
        // GET: api/User
        [Route("api/user/all")]
        [HttpGet]
        public IQueryable<User> GetUsers()
        {
            return db.User;
        }

        // GET: api/User/5
        [HttpGet]
        [Route("api/user/getuserbyid/{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id) {
            User user = db.User.Find(id);
            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        [ResponseType(typeof(User))]
        [Route("api/user/postuser")]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();
            return Ok();
            //return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.UserID == id) > 0;
        }
    }
}