using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ChannelRolePermission {
        public int ChannelId { get; set; }
        public int RoleId { get; set; }
        public string PermissionId { get; set; }
        
        public Channel Channel { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}