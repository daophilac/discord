using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class ChannelRolePermission {
        [Key]
        [Column(Order = 1)]
        public int ChannelId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int RoleId { get; set; }
        [Key]
        [Column(Order = 3)]
        public string PermissionId { get; set; }

        [ForeignKey("ChannelID")]
        public Channel Channel { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }
    }
}
