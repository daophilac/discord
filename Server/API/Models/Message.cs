using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Message {
        [Key]
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        
        public Channel Channel { get; set; }
        public User User { get; set; }
    }
}