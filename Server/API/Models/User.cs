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
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Image { get; set; }

        public IEnumerable<ServerUser> ServerUsers { get; set; }
        public IEnumerable<Message> Messages { get; set; }

        public static User Clone(User userToCopy) {
            return new User() {
                UserId = userToCopy.UserId,
                Email = userToCopy.Email,
                Password = userToCopy.Password,
                Username = userToCopy.Username,
                FirstName = userToCopy.FirstName,
                LastName = userToCopy.LastName,
                Gender = userToCopy.Gender,
                Image = userToCopy.Image,
                ServerUsers = userToCopy.ServerUsers,
                Messages = userToCopy.Messages,
            };
        }
    }
}