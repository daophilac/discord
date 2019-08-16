using System;

namespace Discord.Models {
    public class Message {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }


        public Channel Channel { get; set; }
        public User User { get; set; }
        public Message() { }
        public Message(int channelId, int userId, string content) {
            ChannelId = channelId;
            UserId = userId;
            Content = content;
        }
        public bool SameAs(Message message) {
            if(message == null) {
                return false;
            }
            return MessageId == message.MessageId;
        }
    }
}