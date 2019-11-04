using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Models {
    public class Message {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string MessageId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int ChannelId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public float Technology { get; set; }
        public float Movies { get; set; }
        public float Sport { get; set; }
        public float Economy { get; set; }
        public float Society { get; set; }
        public float Unknown { get; set; }
        public bool Delete { get; set; }
        public bool Violation { get; set; }
        //[BsonIgnore]
        //public Channel Channel { get; set; }
        //[BsonIgnore]
        //public User User { get; set; }
    }
}
