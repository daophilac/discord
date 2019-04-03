using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        MainDatabase context = new MainDatabase(new DbContextOptions<MainDatabase>());
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() {
            User user = new User();
            user.Email = "a";
            user.Password = "123";
            user.UserName = "peanut";
            user.FirstName = "Dao";
            user.LastName = "Lac";
            user.Gender = Gender.Male;
            user.Image = "1.png";
            context.User.Add(user);
            context.SaveChanges();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
