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
        [Required]
        public int ChannelId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime Time { get; set; }

        
        public Channel Channel { get; set; }
        public User User { get; set; }
    }
}