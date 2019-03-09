using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ServerUser {
        [Key]
        [Column(Order = 1)]
        public int ServerID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int UserID { get; set; }


        [ForeignKey("ServerID")]
        public Server Server { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}