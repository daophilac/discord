using System;

namespace Discord_win.Models {
    public class Message {
        public int MessageID { get; set; }
        public int ChannelID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }


        public Channel Channel { get; set; }
        public User User { get; set; }
    }
}
