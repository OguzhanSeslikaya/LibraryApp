using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.Entities.Models
{
    public class StockOutbox
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 0)]
        public string idempotentToken { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement(Order = 1)]
        public DateTime occuredOn { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement(Order = 2)]
        public DateTime? processedDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 3)]
        public string type { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 4)]
        public string payload { get; set; }
    }
}
