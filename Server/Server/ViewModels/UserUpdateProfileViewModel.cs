using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels {
    public class UserUpdateProfileViewModel {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
