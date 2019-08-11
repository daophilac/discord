using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class ResolvedPermission {
        public ServerPermission Server { get; set; }
        public ChannelPermission Channel { get; set; }
        public class ServerPermission {
            public bool Kick { get; set; } = false;
            public bool ModifyChannel { get; set; } = false;
            public bool ModifyRole { get; set; } = false;
        }
        public class ChannelPermission {
            public bool ViewMessage { get; set; } = false;
            public bool SendMessage { get; set; } = false;
            public bool SendImage { get; set; } = false;
            public bool React { get; set; } = false;
        }
    }
}
