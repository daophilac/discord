using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Server {
        public int ServerId { get; set; }
        [MaxLength(50), Required]
        public string ServerName { get; set; }
        [MaxLength(254)]
        public string ImageUrl { get; set; }
        public int? DefaultRoleId { get; set; }
        [Required]
        public int AdminId { get; set; }


        [NotMapped]
        public Role DefaultRole { get; set; }
        public User Admin { get; set; }
        public InstantInvite InstantInvite { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<ServerUser> ServerUsers { get; set; }

        public static Server Clone(Server serverToClone) {
            return new Server {
                ServerId = serverToClone.ServerId,
                ServerName = serverToClone.ServerName,
                ImageUrl = serverToClone.ImageUrl,
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