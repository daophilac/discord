using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Test {
    interface IUserTest {
        List<User> GetUsers();
        User GetUser(int userID);
        string insertUser(User user);
    }
}
