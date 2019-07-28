using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class ServerPermission {
        [Key]
        public int PermissionId { get; set; }
        [Column(TypeName = "VARCHAR(50)"), Required]
        public string PermissionName { get; set; }
        [Column(TypeName = "VARCHAR(1024)")]
        public string Description { get; set; }

        public ICollection<ServerLevelPermission> ServerLevelPermissions { get; set; }
    }
}
