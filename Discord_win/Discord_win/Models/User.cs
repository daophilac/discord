﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public enum Gender {
        Male = 0, Female = 1
    }
    public class User {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string ImageName { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }

        public override bool Equals(object obj) {
            return UserId == ((User)obj).UserId;
        }
        public override int GetHashCode() {
            return UserId.GetHashCode();
        }
    }
}
