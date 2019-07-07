using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Server {
        public int ServerId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int AdminId { get; set; }

        
        public User Admin { get; set; }
        public InstantInvite InstantInvite { get; set; }
        public IEnumerable<ServerUser> ServerUsers { get; set; }
        public IEnumerable<Channel> Channels { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        public static Server Clone(Server serverToClone) {
            return new Server {
                ServerId = serverToClone.ServerId,
                Name = serverToClone.Name,
                Image = serverToClone.Image,
                AdminId = serverToClone.AdminId,
                Admin = serverToClone.Admin,
                InstantInvite = serverToClone.InstantInvite,
                ServerUsers = serverToClone.ServerUsers,
                Channels = serverToClone.Channels,
                Roles = serverToClone.Roles,
            };
        }
    }
}