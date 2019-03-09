using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class ChannelRolePermission {
        [Key]
        [Column(Order = 1)]
        public int ChannelID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int RoleID { get; set; }
        [Key]
        [Column(Order = 3)]
        public string PermissionID { get; set; }

        [ForeignKey("ChannelID")]
        public Channel Channel { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }
    }
}