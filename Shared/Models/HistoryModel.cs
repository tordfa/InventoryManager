using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorApp1.Shared.Models
{
    public class HistoryModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string history { get; set; }

    }
}
