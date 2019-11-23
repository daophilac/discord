using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels {
    public static class Mapper {
        public static void Map(this UserUpdateProfileViewModel userUpdateProfileViewModel, User user) {
            user.UserName = userUpdateProfileViewModel.Username;
            user.Email = userUpdateProfileViewModel.Email;
        }
    }
}
