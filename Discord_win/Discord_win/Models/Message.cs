using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Message {
        [Key]
        public int MessageID { get; set; }
        public int ChannelID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }


        [ForeignKey("ChannelID")]
        public Channel Channel { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
