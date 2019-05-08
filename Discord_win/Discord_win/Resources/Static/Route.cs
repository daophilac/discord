using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Resources.Static {
    static class Route {
        public static readonly string Protocol = "http";

        public static readonly string DomainName = "192.168.2.109";
        //public static readonly string DomainName = "192.168.43.72";
        //public static readonly string DomainName = "localhost:44334";

        public static readonly string ServerName = "/sv";
        //public static readonly string ServerName = "";

        public static readonly string URILogin = "/api/user/login";
        public static readonly string URIGetServersByUser = "/api/server/getserversbyuser/{0}";
        public static readonly string URIGetChannelsByServer = "/api/channel/getchannelsbyserver/{0}";
        public static readonly string URIGetMessagesByChannel = "/api/message/getmessagesbychannel/{0}";
        public static readonly string URIInsertMessage = "/api/message/insertmessage";
        public static readonly string URIChatHub = "/chathub";
    }
}
