using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class ServerUser {
        [Key]
        [Column(Order = 1)]
        public int ServerId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }


        [ForeignKey("ServerID")]
        public Server Server { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
