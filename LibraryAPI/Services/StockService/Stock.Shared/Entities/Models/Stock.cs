using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.Entities.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 0)]
        public string stockId { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 1)]
        public string bookName { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        [BsonElement(Order = 2)]
        public int totalQuantity { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        [BsonElement(Order = 3)]
        public int currentQuantity { get; set; }

    }
}
