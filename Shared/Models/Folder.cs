using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorApp1.Shared.Models
{
    public class Folder
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ParentFolderId { get;set; }

        public List<string> ChildrenFolders { get; set; } = new List<string>();
        public List<string> Items { get; set; } = new List<string>();
    }
}
