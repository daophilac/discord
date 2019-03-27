using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Channel {
        public int ChannelID { get; set; }
        public string Name { get; set; }
        public int ServerID { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
