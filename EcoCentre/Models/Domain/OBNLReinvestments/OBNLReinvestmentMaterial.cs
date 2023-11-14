using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.OBNLReinvestments
{
    public class OBNLReinvestmentMaterial
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MaterialId { get; set; }
        
        public double Weight { get; set; }
    }
}