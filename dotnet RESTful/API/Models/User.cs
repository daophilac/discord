using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public enum Gender {
        Male, Female
    }
    public class User {
        [Key]
        public int UserID { get; set; }
        [MaxLength(256)]
        [Index("IX_Email", 1, IsUnique = true)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Image { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}