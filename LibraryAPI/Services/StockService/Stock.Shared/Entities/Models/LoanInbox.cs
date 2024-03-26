using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.Entities.Models
{
    public class LoanInbox
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 0)]
        public string idempotentToken { get; set; }
        [BsonRepresentation(BsonType.Boolean)]
        [BsonElement(Order = 1)]
        public bool processed { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 2)]
        public string payload { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 3)]
        public string typeName { get; set; }
    }
}
