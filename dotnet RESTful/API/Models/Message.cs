using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Message {
        public int ServerID { get; set; }
        public int ChannelID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}