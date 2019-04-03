using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Channel {
        [Key]
        public int ChannelID { get; set; }
        public string Name { get; set; }
        public int ServerID { get; set; }

        public IEnumerable<ChannelRolePermission> ChannelRolePermissions { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public Server Server { get; set; }
    }
}