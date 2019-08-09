using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Models {
    public class Role {
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
        public bool MainRole { get; set; }
        public string RoleName { get; set; }
        public bool Kick { get; set; }
        public bool ModifyChannel { get; set; }
        public bool ModifyRole { get; set; }
        public bool ChangeUserRole { get; set; }
        public int ServerId { get; set; }

        public Server Server { get; set; }
        public ICollection<ChannelPermission> ChannelPermissions { get; set; }
        public static bool operator <(Role a, Role b) {
            return a.RoleLevel < b.RoleLevel;
        }
        public static bool operator <=(Role a, Role b) {
            return a.RoleLevel <= b.RoleLevel;
        }
        public static bool operator >(Role a, Role b) {
            return a.RoleLevel > b.RoleLevel;
        }
        public static bool operator >=(Role a, Role b) {
            return a.RoleLevel >= b.RoleLevel;
        }
    }
}
