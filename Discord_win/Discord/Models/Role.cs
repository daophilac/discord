using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public class Role {
        public readonly int HighestRoleLevel = 1000;
        public bool IsHighestLevel { get => RoleLevel == HighestRoleLevel; }
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
        public bool MainRole { get; set; }
        public string RoleName { get; set; }
        public bool Kick { get; set; }
        public bool ManageChannel { get; set; }
        public bool ManageRole { get; set; }
        public bool ChangeUserRole { get; set; }
        public int ServerId { get; set; }

        public Server Server { get; set; }
        public ICollection<ChannelPermission> ChannelPermissions { get; set; }
        public void AssignPropertiesFrom(Role source) {
            RoleLevel = source.RoleLevel;
            RoleName = source.RoleName;
            Kick = source.Kick;
            ManageChannel = source.ManageChannel;
            ManageRole = source.ManageRole;
            ChangeUserRole = source.ChangeUserRole;
        }
        public bool SameAs(Role role) {
            if(role == null) {
                return false;
            }
            return RoleId == role.RoleId;
        }
        public bool LowerThan(Role role) {
            return RoleLevel < role.RoleLevel;
        }
        public bool HigherThan(Role role) {
            return RoleLevel > role.RoleLevel;
        }
        public bool EqualLevel(Role role) {
            return RoleLevel == role.RoleLevel;
        }
    }
}
