using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ServerUser {
        public int ServerID { get; set; }
        public int UserID { get; set; }


        public Server Server { get; set; }
        public User User { get; set; }
    }
}