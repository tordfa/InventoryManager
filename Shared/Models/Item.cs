using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace BlazorApp1.Shared.Models
{
    public class Item
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; } = "Item Description";
        public decimal? Price { get; set; } = 0;
        public List<ItemTag> Tags { get; set; } = new List<ItemTag>();
        public string ParentFolder { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int SerialNumber { get; set; } = 0;
        public string ImgSrc { get; set; } = "https://www.kolbotntannlegesenter.no/wp-content/uploads/2016/10/orionthemes-placeholder-image-300x300.png";



    }
}