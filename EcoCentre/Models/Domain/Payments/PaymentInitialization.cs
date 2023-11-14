using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Payments
{
	public class PaymentInitialization : Entity
	{
		public string Reference { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string InvoiceId { get; set; }

		public DateTime DateInserted { get; set; }
		public UserInfo User { get; set; }
		public List<string> Responses { get; set; }
	}
}