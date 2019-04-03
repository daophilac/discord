using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Server {
        [Key]
        public int ServerID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int AdminID { get; set; }

        
        public User Admin { get; set; }
        public IEnumerable<ServerUser> ServerUsers { get; set; }
        public IEnumerable<Channel> Channels { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}