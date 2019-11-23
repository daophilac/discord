using System;
using System.Collections.Generic;
using System.Text;

namespace Models {
    public class ClientUser {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }
        public int ViolationId { get; set; }

        //public static User Clone(User userToCopy) {
        //    return new User() {
        //        UserId = userToCopy.UserId,
        //        Email = userToCopy.Email,
        //        Password = userToCopy.Password,
        //        UserName = userToCopy.UserName,
        //        ImageName = userToCopy.ImageName,
        //        ServerUsers = userToCopy.ServerUsers
        //        //Messages = userToCopy.Messages,
        //    };
        //}
    }
}
