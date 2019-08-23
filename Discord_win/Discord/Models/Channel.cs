using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public class Channel {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int ServerId { get; set; }

        public Server Server { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChannelPermission> ChannelPermissions { get; set; }
        public void UpdateInfo(Channel source) {
            ChannelName = source.ChannelName;
        }
        public bool SameAs(Channel channel) {
            return ChannelId == channel.ChannelId;
        }
        public Channel Clone() {
            return MemberwiseClone() as Channel;
        }
    }
}
