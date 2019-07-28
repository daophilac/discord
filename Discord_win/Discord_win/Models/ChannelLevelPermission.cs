using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
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
