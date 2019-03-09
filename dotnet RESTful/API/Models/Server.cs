using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Server {
        public int ServerID { get; set; }
        public string Image { get; set; }
        public int AdminID { get; set; }


        public User Admin { get; set; }
        public ICollection<Channel> Channels { get; set; }
    }
}