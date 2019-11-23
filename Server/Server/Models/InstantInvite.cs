using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models {
    public class InstantInvite {
        [Key, Column(TypeName = "VARCHAR(50)")]
        public string Link { get; set; }
        [Required]
        public int ServerId { get; set; }
        [Required]
        public bool StillValid { get; set; }
        [Required]
        public bool NerverExpired { get; set; }

        public Server Server { get; set; }
    }
}
