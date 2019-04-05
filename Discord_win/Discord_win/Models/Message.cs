using System;

namespace Discord_win.Models {
    public class Message {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }


        public Channel Channel { get; set; }
        public User User { get; set; }
    }
}