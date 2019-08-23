using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public class Server {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ImageName { get; set; }
        public int? DefaultRoleId { get; set; }
        public int AdminId { get; set; }


        public Role DefaultRole { get; set; }
        public User Admin { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }
        public override bool Equals(object obj) {
            if(obj is Server) {
                return ((Server)obj).ServerId == ServerId;
            }
            return false;
        }
        public override int GetHashCode() {
            return ServerId.GetHashCode();
        }
        public static bool operator ==(Server a, Server b) {
            object oa = a as object;
            object ob = b as object;
            if(oa == null && ob == null) {
                return true;
            }
            else if(oa != null && ob != null) {
                return a.ServerId == b.ServerId;
            }
            else {
                return false;
            }
        }
        public static bool operator !=(Server a, Server b) {
            object oa = a as object;
            object ob = b as object;
            if (oa == null && ob == null) {
                return false;
            }
            else if (oa != null && ob != null) {
                return a.ServerId != b.ServerId;
            }
            else {
                return true;
            }
        }
    }
}
