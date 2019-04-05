using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public enum Gender {
        Male, Female
    }
    public class User {
        [Key]
        public int UserId { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Image { get; set; }

        public IEnumerable<Server> Servers { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}