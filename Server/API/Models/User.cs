using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public enum Gender {
        Male, Female, Unknown
    }
    public class User {
        [Key]
        public int UserId { get; set; }
        [Column(TypeName = "VARCHAR(254)"), Required]
        public string Email { get; set; }
        [Column(TypeName = "VARCHAR(15)")]
        public string Phone { get; set; }
        [Column(TypeName = "VARCHAR(60)"), Required]
        public string Password { get; set; }
        [Column(TypeName = "VARCHAR(50)"), Required]
        public string UserName { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [MaxLength(254)]
        public string ImageName { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }

        public static User Clone(User userToCopy) {
            return new User() {
                UserId = userToCopy.UserId,
                Email = userToCopy.Email,
                Password = userToCopy.Password,
                UserName = userToCopy.UserName,
                FirstName = userToCopy.FirstName,
                LastName = userToCopy.LastName,
                Gender = userToCopy.Gender,
                ImageName = userToCopy.ImageName,
                ServerUsers = userToCopy.ServerUsers,
                Messages = userToCopy.Messages,
            };
        }
    }
}