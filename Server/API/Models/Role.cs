using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Role {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int ServerId { get; set; }

        
        public IEnumerable<ChannelRolePermission> ChannelRolePermissions { get; set; }
        public Server Server { get; set; }
    }
}