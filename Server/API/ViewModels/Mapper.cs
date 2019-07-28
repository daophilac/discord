using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels {
    public static class Mapper {
        public static void Map(this UserUpdateProfileViewModel userUpdateProfileViewModel, User user) {
            user.Username = userUpdateProfileViewModel.Username;
            user.Email = userUpdateProfileViewModel.Email;
        }
    }
}
