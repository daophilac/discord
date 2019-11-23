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
using Peanut.Server;

namespace API.Controllers {
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly MainDatabase database;
        private readonly FileProvider fileProvider;
        public UserController(MainDatabase database, FileProvider fileProvider) {
            this.database = database;
            this.fileProvider = fileProvider;
        }
        [HttpPost, HttpHead, Route("UploadImage/{userId}")]
        public async Task<IActionResult> UploadFile(int userId) {
            if (!Request.HasFormContentType) {
                return BadRequest("Must be a form-data request.");
            }
            if (Request.Form == null) {
                return BadRequest("form-data is empty.");
            }
            IFormFileCollection formFiles = Request.Form.Files;
            foreach (IFormFile formFile in formFiles) {
                if(!await fileProvider.Get(formFile, FileSystem.BuildUserImagePath(formFile.FileName, userId), true)) {
                    return BadRequest();
                }
            }
            return Ok();
        }
        [HttpGet, HttpHead, Route("DownloadImage/{imageName}")]
        public async Task<IActionResult> Download(string imageName) {
            if(imageName == null) {
                return BadRequest();
            }
            User user = await database.User.Where(u => u.ImageName == imageName).FirstOrDefaultAsync();
            if(user == null) {
                return NotFound();
            }
            string fullPath = FileSystem.GetUserImagePath(imageName);
            return fileProvider.Send(fullPath);
        }
        [HttpGet, HttpHead, Route("testdownload")]
        public IActionResult TestDownload() {
            string fullPath = Path.Combine(FileSystem.UserImageDirectory, "a.zip");
            return fileProvider.Send(fullPath);
        }
        [HttpPost, Route("testupload")]
        public async Task<IActionResult> TestUpload(List<IFormFile> files) {
            if (!Request.HasFormContentType) {
                return BadRequest("Must be a form-data request.");
            }
            if (Request.Form == null) {
                return BadRequest("form-data is empty.");
            }
            if(Request.Form.Files.Count == 0) {
                return BadRequest();
            }
            await fileProvider.Get(Request.Form.Files[0], "D:/Desktop/a.zip", true);
            return Ok();
        }
        [HttpPost, Route("testupload2")]
        public IActionResult TestUpload2() {
            return Ok();
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login(UserLoginViewModel requestedUser) {
            var users = database.User.Where(u => u.Email == requestedUser.Email && u.Password == requestedUser.Password);
            if (!users.Any()) {
                return NotFound();
            }
            User user = await users.FirstAsync();
            if (!await CheckUserViolation(user)) {
                return Ok(user);
            }
            return StatusCode(403);
        }
        private async Task<bool> CheckUserViolation(User user) {
            if(user.ViolationId == 0) {
                return false;
            }
            Violation violation = await database.Violation.FindAsync(user.ViolationId);
            if(violation.TimeEnd.Value.CompareTo(DateTime.Now) < 0) {
                user.ViolationId = 0;
                await database.SaveChangesAsync();
                return false;
            }
            return true;
        }
        [HttpPost, Route("SignUp")]
        public ActionResult<User> SignUp(User userFromClient) {
            database.User.Add(userFromClient);
            database.SaveChanges();
            return database.User.Last();
        }
        [HttpPost, Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileViewModel userUpdateProfileViewModel) {
            User user = await database.User.FindAsync(userUpdateProfileViewModel.UserId);
            if(user == null) {
                return NotFound();
            }
            userUpdateProfileViewModel.Map(user);
            await database.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost, Route("ConfirmPassword")]
        public async Task<IActionResult> ConfirmPassword(UserConfirmPasswordViewModel userVM) {
            User user = await database.User.Where(u => u.UserId == userVM.UserId && u.Password == userVM.Password).FirstOrDefaultAsync();
            if(user == null) {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet, Route("CheckUnavailableEmail/{email}")]
        public async Task<IActionResult> CheckUnavailableEmail(string email) {
            User user = await database.User.Where(u => u.Email == email).FirstOrDefaultAsync();
            if(user == null) {
                return Ok();
            }
            return Conflict();
        }
        [HttpGet, Route("GetByServer/{serverId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetByServer(int serverId) {
            return await database.User.Where(u => u.ServerUsers.Any(su => su.ServerId == serverId)).Include("ServerUsers").ToListAsync();
        }





        // GET: api/User
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser() {
            return await database.User.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet, Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await database.User.FindAsync(id);

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

            database.Entry(user).State = EntityState.Modified;

            try {
                await database.SaveChangesAsync();
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
            database.User.Add(user);
            await database.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            var user = await database.User.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            database.User.Remove(user);
            await database.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id) {
            return database.User.Any(e => e.UserId == id);
        }
    }
}
