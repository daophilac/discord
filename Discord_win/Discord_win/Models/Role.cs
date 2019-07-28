using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Role {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ServerId { get; set; }

        
        public Server Server { get; set; }
        public ICollection<ServerLevelPermission> ServerLevelPermissions { get; set; }
        public ICollection<ChannelLevelPermission> ChannelLevelPermissions { get; set; }
    }
}
