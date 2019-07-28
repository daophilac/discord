using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class ServerPermission {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }

        public ICollection<ServerLevelPermission> ServerLevelPermissions { get; set; }
    }
}
