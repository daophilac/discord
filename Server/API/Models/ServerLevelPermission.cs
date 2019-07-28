using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class ServerLevelPermission {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public Role Role { get; set; }
        public ServerPermission ServerPermission { get; set; }
    }
}
