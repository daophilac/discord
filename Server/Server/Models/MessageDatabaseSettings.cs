using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models {
    public class MessageDatabaseSettings : IMessageDatabaseSettings {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MessageCollectionName { get; set; }
    }
    public interface IMessageDatabaseSettings {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string MessageCollectionName { get; set; }
    }
}
