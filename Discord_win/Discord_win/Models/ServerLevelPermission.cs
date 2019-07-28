using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class ServerLevelPermission {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsActive { get; set; }

        public Role Role { get; set; }
        public ServerPermission ServerPermission { get; set; }
    }
}
