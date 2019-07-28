using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ChannelLevelPermission {
        public int ChannelId { get; set; }
        public int RoleId { get; set; }
        public int ChannelPermissionId { get; set; }
        public bool IsActive { get; set; }
        
        public Channel Channel { get; set; }
        public Role Role { get; set; }
        public ChannelPermission ChannelPermission { get; set; }
    }
}