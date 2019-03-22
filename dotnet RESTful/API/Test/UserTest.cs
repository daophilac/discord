using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Test {
    public class UserTest : IUserTest {
        private APIContext db = new APIContext();
        public User GetUser(int userID) {
            return db.User.Find(userID);
            //throw new NotImplementedException();
        }

        public List<User> GetUsers() {
            return db.User.ToList();
            //throw new NotImplementedException();
        }

        public string insertUser(User user) {
            APIContext contextTemp = new APIContext();
            User userTemp = new User();
            userTemp.Email = user.Email;
            userTemp.Password = user.Password;
            userTemp.UserName = user.UserName;
            userTemp.FirstName = user.FirstName;
            userTemp.LastName = user.LastName;
            userTemp.Gender = user.Gender;
            userTemp.Image = user.Image;
            //userTemp.Email = "daophilac99@gmail.com";
            //userTemp.Password = "daophilac99@gmail.com";
            //userTemp.UserName = "daophilac99@gmail.com";
            //userTemp.FirstName = "daophilac99@gmail.com";
            //userTemp.LastName = "daophilac99@gmail.com";
            //userTemp.Gender = Gender.Male;
            //userTemp.Image = "daophilac99@gmail.com";
            contextTemp.User.Add(userTemp);
            contextTemp.SaveChanges();
            return "Hey!! congrates.. Amit your User data is saved successfully";
            //throw new NotImplementedException();
        }
    }
}