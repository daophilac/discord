using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Server {
        public int ServerId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int AdminId { get; set; }


        public User Admin { get; set; }
        public IEnumerable<ServerUser> ServerUsers { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Channel> Channels { get; set; }
    }
}
