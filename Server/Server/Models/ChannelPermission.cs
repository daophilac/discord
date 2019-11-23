using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ChannelPermission {
        public int ChannelId { get; set; }
        public int RoleId { get; set; }
        public bool ViewMessage { get; set; }
        public bool React { get; set; }
        public bool SendMessage { get; set; }
        public bool SendImage { get; set; }
        
        public Channel Channel { get; set; }
        public Role Role { get; set; }
        public bool SameAs(ChannelPermission channelPermission) {
            return ChannelId == channelPermission.ChannelId &&
                RoleId == channelPermission.RoleId;
        }
        public void UpdateFrom(ChannelPermission source) {
            ViewMessage = source.ViewMessage;
            React = source.React;
            SendMessage = source.SendMessage;
            SendImage = source.SendImage;
        }
    }
}