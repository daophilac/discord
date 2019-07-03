using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Permission {
        [Key]
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<ChannelRolePermission> ChannelRolePermissions { get; set; }
    }
}