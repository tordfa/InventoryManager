using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
namespace BlazorApp1.Shared.Models {
    public class DBContext { 
        private readonly IMongoDatabase _mongoDatabase; 
        public DBContext() { 
            var client = new MongoClient("mongodburl");
            _mongoDatabase = client.GetDatabase("Inventory");
        }
        public IMongoCollection<Item> ItemRecord {get { return _mongoDatabase.GetCollection<Item>("items");}} 

        public IMongoCollection<Folder> FolderRecord{get { return _mongoDatabase.GetCollection<Folder>("folders");}}

        public IMongoCollection<HistoryModel> HistoryCollection { get { return _mongoDatabase.GetCollection<HistoryModel>("history"); } }
        public IMongoCollection<ItemTag> TagCollection { get { return _mongoDatabase.GetCollection<ItemTag>("tags"); } }
        public IMongoCollection<UserCredentialsModel> UserCollection { get { return _mongoDatabase.GetCollection<UserCredentialsModel>("users"); } }

        public IMongoCollection<AccountModel> AccountCollection { get { return _mongoDatabase.GetCollection<AccountModel>("accounts"); } }


    } 
}
