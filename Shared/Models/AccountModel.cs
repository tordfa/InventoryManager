using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared.Models
{
    public class AccountModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<Folder> folders { get; set; } = new List<Folder>();
        public List<HistoryModel> history { get; set; } = new List<HistoryModel>();
        public List<Item> items { get; set; } = new List<Item>();
        public List<ItemTag> tags { get; set; } = new List<ItemTag>();
    }
}
