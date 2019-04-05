using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Models {
    public class Role {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int ServerId { get; set; }


        [ForeignKey("ServerID")]
        public Server Server { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
