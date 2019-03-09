using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Models {
    public class APIContextInitializer : System.Data.Entity.CreateDatabaseIfNotExists<APIContext> {
        protected override void Seed(APIContext context) {
            var users = new List<User> {
                new User{Email="daophilac@gmail.com",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null},
                new User{Email="daophilac1@gmail.com",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null},
                new User{Email="lucknight@gmail.com",UserName="lucknight",FirstName="luck",LastName="night",Gender=Gender.Male,Image=null},
                new User{Email="eddie@gmail.com",UserName="eddie",FirstName="ed",LastName="die",Gender=Gender.Male,Image=null}
            };
            users.ForEach(u => context.User.Add(u));
            context.SaveChanges();

            var servers = new List<Server> {
                new Server{Name="Final Fantasy",AdminID=1},
                new Server{Name="Ys",AdminID=1},
                new Server{Name="Hentai Maiden",AdminID=2}
            };
            servers.ForEach(s => context.Server.Add(s));
            context.SaveChanges();
        }
    }
}