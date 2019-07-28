using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Channel {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int ServerId { get; set; }

        public Server Server { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChannelLevelPermission> ChannelLevelPermissions { get; set; }
        public Channel() {

        }
        public Channel(string name, int serverId) {
            ChannelName = name;
            ServerId = serverId;
        }
    }
}
