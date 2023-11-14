using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.OBNLReinvestments
{
	public class OBNLReinvestmentCreator
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }
		public string UserName { get; set; }
	}
}