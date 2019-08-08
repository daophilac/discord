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
        public string ImageUrl { get; set; }
        public int? DefaultRoleId { get; set; }
        public int AdminId { get; set; }


        public Role DefaultRole { get; set; }
        public User Admin { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }
    }
}
