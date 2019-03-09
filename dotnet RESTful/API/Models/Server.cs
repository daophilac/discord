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


        [ForeignKey("AdminID")]
        public User User { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<User> Users { get; set; }
    }
}