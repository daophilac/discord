using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models {
    public enum Gender {
        Male, Female
    }
    public class User {
        public int ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Image { get; set; }
    }
}