using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Role {
        [Key]
        public int RoleID { get; set; }
        public string Name { get; set; }
        public int ServerID { get; set; }


        [ForeignKey("ServerID")]
        public Server Server { get; set; }
        public ICollection<User> Users { get; set; }
    }
}