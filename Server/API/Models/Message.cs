using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models {
    public class Message {
        //[Key]
        //public int MessageId { get; set; }
        //[Required]
        //public int ChannelId { get; set; }
        //[Required]
        //public int UserId { get; set; }
        //public string Content { get; set; }
        //[Required]
        //public DateTime Time { get; set; }


        //public Channel Channel { get; set; }
        //public User User { get; set; }
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string MessageId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int ChannelId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int UserId { get; set; }
        private string content;
        public string Content {
            get => content;
            set {
                content = value;
                SelfClassify();
            }
        }
        public DateTime Time { get; set; }
        public string Category {
            get {
                float[] _ = new float[] { Technology, Movies, Sport, Economy, Society, Unknown };
                int indexMax = 0;
                float max = Technology;
                for (int i = 1; i < 6; i++) {
                    if (_[i] > max) {
                        indexMax = i;
                        max = _[i];
                    }
                }
                switch (indexMax) {
                    case 0:
                        return "Technology";
                    case 1:
                        return "Movies";
                    case 2:
                        return "Sport";
                    case 3:
                        return "Economy";
                    case 4:
                        return "Society";
                    case 5:
                        return "Unknown";
                    default:
                        return null;
                }
            }
        }
        public float Technology { get; set; }
        public float Movies { get; set; }
        public float Sport { get; set; }
        public float Economy { get; set; }
        public float Society { get; set; }
        public float Unknown { get; set; }
        public bool Delete { get; set; }
        public bool Violation { get; set; }
        [BsonIgnore]
        public Channel Channel { get; set; }
        [BsonIgnore]
        public User User { get; set; }
        public void SelfClassify() {
            //
            Technology = 1;
        }
    }
}