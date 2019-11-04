using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class MongoContext : IMongoContext {
        public IMongoCollection<Message> Messages { get; }
        public MongoContext(IMessageDatabaseSettings settings) {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            Messages = database.GetCollection<Message>(settings.MessageCollectionName);
        }
    }
    public interface IMongoContext {
        IMongoCollection<Message> Messages { get; }
    }
}
