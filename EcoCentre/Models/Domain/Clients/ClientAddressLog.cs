using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Clients
{
	public class  ClientAddressLog
	{
		public DateTime ChangedAt { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string AddressId { get; set; }
	}
}