using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.IO;
using API.Tools;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MainDatabase _context;

        public UserController(MainDatabase context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("longtask")]
        public IActionResult LongTask() {
            string filePath = "D:/Desktop/repos/discord/Server/API/Images/User/big1.iso";
            return PhysicalFile(filePath, "compressed/iso");
        }






        [HttpGet]
        [Route("downloadbigfile/{filename}")]
        public IActionResult DownloadBigFile(string filename) {
            string filePath = "D:/Desktop/repos/discord/Server/API/Images/User/" + filename;
            return PhysicalFile(filePath, "compressed/iso");
        }









        [HttpGet]
        [Route("testdownload/{filename}")]
        public IActionResult Download(string filename) {
            string fullPath = FileSystem.GetUserImagePath(filename);
            return Program.fileCreator.Send(fullPath);
        }
        [HttpGet]
        [Route("downloadimage/{userId}")]
        public IActionResult Download(int userId) {
            string filename = _context.User.Find(userId).Image;
            if (filename == null) {
                return NotFound();
            }
            string fullPath = FileSystem.GetUserImagePath(filename);
            return Program.fileCreator.Send(fullPath);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<User> Login(User requestedUser) {
            var users = _context.User.Where(u => u.Email == requestedUser.Email && u.Password == requestedUser.Password);
            if(users.Count() == 0) {
                return NoContent();
            }
            return users.First();
        }
        [HttpPost]
        [Route("signup")]
        public ActionResult<User> SignUp(User userFromClient) {
            _context.User.Add(userFromClient);
            _context.SaveChanges();
            return _context.User.Last();
        }









        // GET: api/User
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/User/5
        //[HttpGet("{id}")]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        [Route("api/user/post")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
