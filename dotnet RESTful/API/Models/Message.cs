using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Message {
        [Key]
        public int ChannelID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}