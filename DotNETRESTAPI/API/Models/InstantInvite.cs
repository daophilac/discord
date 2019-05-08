using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class InstantInvite {
        [Key]
        public string Link { get; set; }
        public int ServerId { get; set; }
        public bool NerverExpire { get; set; }

        public Server Server { get; set; }
    }
}
