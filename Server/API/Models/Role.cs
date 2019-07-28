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
        [MaxLength(50), Required]
        public string RoleName { get; set; }
        [Required]
        public int ServerId { get; set; }
        
        public Server Server { get; set; }
        public ICollection<ServerLevelPermission> ServerLevelPermissions { get; set; }
        public ICollection<ChannelLevelPermission> ChannelLevelPermissions { get; set; }
    }
}