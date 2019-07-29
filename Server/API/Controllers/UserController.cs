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
using System.Net.Http;
using API.ViewModels;

namespace API.Controllers {
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly MainDatabase _context;
        public UserController(MainDatabase context) {
            _context = context;
        }
        [HttpPost, HttpHead, Route("uploadimage/{userId}")]
        public async Task<IActionResult> UploadFile(IFormFileCollection formFileCollection, int userId) {
            if(Request.Form == null) {
                return BadRequest();
            }
            IFormFileCollection formFiles = Request.Form.Files;
            foreach (IFormFile formFile in formFiles) {
                if(await Program.fileProvider.Get(formFile, FileSystem.BuildUserImagePath(formFile.FileName, userId), true) != Ok()) {
                    return BadRequest();
                }
            }
            return Ok();
        }
        [HttpGet, HttpHead, Route("downloadimage/{imageName}")]
        public async Task<IActionResult> Download(string imageName) {
            if(imageName == null) {
                return BadRequest();
            }
            User user = await _context.User.Where(u => u.ImageName == imageName).FirstOrDefaultAsync();
            if(user == null) {
                return NotFound();
            }
            string fullPath = FileSystem.GetUserImagePath(imageName);
            return Program.fileProvider.Send(fullPath);
        }
        [HttpGet, HttpHead, Route("testdownload")]
        public IActionResult TestDownload() {
            string fullPath = Path.Combine(FileSystem.UserImageDirectory, "a.zip");
            return Program.fileProvider.Send(fullPath);
        }

        [HttpPost, Route("login")]
        public ActionResult<User> Login(UserLoginViewModel requestedUser) {
            var users = _context.User.Where(u => u.Email == requestedUser.Email && u.Password == requestedUser.Password);
            if (users.Count() == 0) {
                return NoContent();
            }
            return users.First();
        }
        [HttpPost, Route("signup")]
        public ActionResult<User> SignUp(User userFromClient) {
            _context.User.Add(userFromClient);
            _context.SaveChanges();
            return _context.User.Last();
        }
        [HttpPost, Route("updateprofile")]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileViewModel userUpdateProfileViewModel) {
            User user = await _context.User.FindAsync(userUpdateProfileViewModel.UserId);
            if(user == null) {
                return NotFound();
            }
            userUpdateProfileViewModel.Map(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost, Route("confirmpassword")]
        public async Task<IActionResult> ConfirmPassword(UserConfirmPasswordViewModel userVM) {
            User user = await _context.User.Where(u => u.UserId == userVM.UserId && u.Password == userVM.Password).FirstOrDefaultAsync();
            if(user == null) {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet, Route("checkunavailableemail/{email}")]
        public async Task<IActionResult> CheckUnavailableEmail(string email) {
            User user = await _context.User.Where(u => u.Email == email).FirstOrDefaultAsync();
            if(user == null) {
                return Ok();
            }
            return Conflict();
        }






        // GET: api/User
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser() {
            return await _context.User.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet, Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _context.User.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user) {
            if (id != user.UserId) {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        [Route("api/user/post")]
        public async Task<ActionResult<User>> PostUser(User user) {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            var user = await _context.User.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id) {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
