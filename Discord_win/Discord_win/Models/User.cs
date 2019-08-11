using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public enum Gender {
        Male = 0, Female = 1
    }
    public class User {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ImageName { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }
        public override bool Equals(object obj) {
            if(obj is User) {
                return UserId == ((User)obj).UserId;
            }
            return false;
        }
        public override int GetHashCode() {
            return UserId.GetHashCode();
        }
        public static bool operator ==(User a, User b) {
            object oa = a as object;
            object ob = b as object;
            if (oa == null && ob == null) {
                return true;
            }
            else if (oa != null && ob != null) {
                return a.UserId == b.UserId;
            }
            else {
                return false;
            }
        }
        public static bool operator !=(User a, User b) {
            object oa = a as object;
            object ob = b as object;
            if (oa == null && ob == null) {
                return false;
            }
            else if (oa != null && ob != null) {
                return a.UserId != b.UserId;
            }
            else {
                return true;
            }
        }
    }
}
