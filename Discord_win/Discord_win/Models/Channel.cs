using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Channel {
        public int ChannelId { get; set; }
        public string Name { get; set; }
        public int ServerId { get; set; }

        public ICollection<Message> Messages { get; set; }
        public Channel() {

        }
        public Channel(string name, int serverId) {
            Name = name;
            ServerId = serverId;
        }
    }
}
