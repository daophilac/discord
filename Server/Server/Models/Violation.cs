using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models {
    public class Violation {
        public int ViolationId { get; set; }
        public string Message { get; set; } = "Violate terms of use";
        [Required]
        public bool Warned { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

    }
}
