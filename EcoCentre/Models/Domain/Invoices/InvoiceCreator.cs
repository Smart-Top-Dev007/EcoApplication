using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Invoices
{
	public class InvoiceCreator
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }
		public string UserName { get; set; }
	}
}