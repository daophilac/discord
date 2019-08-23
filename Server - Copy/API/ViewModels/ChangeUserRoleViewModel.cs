using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels {
    public class ChangeUserRoleViewModel {
        public int ServerId { get; set; }
        public int UserId { get; set; }
        public int NewRoleId { get; set; }
    }
}
