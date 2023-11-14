using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Invoices
{
	[BsonIgnoreExtraElements]
    public class InvoiceMaterial
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MaterialId { get; set; }

        public decimal Quantity { get; set; }
        public double Weight { get; set; }
	    public decimal Price { get; set; }
	    public decimal Amount { get; set; }
	    public bool IsUsingFreeAmount { get; set; }
	    public string Container { get; set; }
    }
}