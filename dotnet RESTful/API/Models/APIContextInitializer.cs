using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Models {
    public class APIContextInitializer : System.Data.Entity.DropCreateDatabaseAlways<APIContext> {
        protected override void Seed(APIContext context) {
            var users = new List<User> {
                new User{Email="daophilac@gmail.com",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null},
                new User{Email="daophilac@gmail.com2",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null}
            };
            users.ForEach(u => context.User.Add(u));
            context.SaveChanges();
        }
    }
}