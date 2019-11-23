using Newtonsoft.Json;
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
        [Required]
        public int RoleLevel { get; set; }
        [Required]
        public bool MainRole { get; set; }
        [MaxLength(50), Required]
        public string RoleName { get; set; }
        public bool Kick { get; set; }
        public bool ManageChannel { get; set; }
        public bool ManageRole { get; set; }
        public bool ChangeUserRole { get; set; }
        [Required]
        public int ServerId { get; set; }
        
        [JsonIgnore]
        public Server Server { get; set; }
        public ICollection<ChannelPermission> ChannelPermissions { get; set; }
        public bool SameServerWith(Role role) {
            return ServerId == role.ServerId;
        }
        public bool SameInServer(Role role) {
            return ServerId == role.ServerId && RoleLevel == role.RoleLevel;
        }
        public bool SameLevelWith(Role role) {
            return RoleLevel == role.RoleLevel;
        }
    }
}