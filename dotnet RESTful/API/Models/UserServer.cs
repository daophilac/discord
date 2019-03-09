using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class UserServer {
        [Key]
        [Column(Order = 1)]
        public int UserID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ServerID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
        [ForeignKey("ServerID")]
        public Server Server { get; set; }
    }
}