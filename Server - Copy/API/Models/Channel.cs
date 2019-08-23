using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Channel {
        [Key]
        public int ChannelId { get; set; }
        [MaxLength(50), Required]
        public string ChannelName { get; set; }
        [Required]
        public int ServerId { get; set; }

        public Server Server { get; set; }
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChannelPermission> ChannelLevelPermissions { get; set; }
        public void UpdateInfo(Channel source) {
            ChannelName = source.ChannelName;
        }
    }
}