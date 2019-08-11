using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public class ServerUser {
        public int ServerId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        
        public Server Server { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
