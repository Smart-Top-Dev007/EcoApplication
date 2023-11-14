using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Containers
{
	[BsonIgnoreExtraElements]
	public class Container : Entity
	{
		public string Number { get; set; }
		public decimal Capacity { get; set; }
		public decimal FillAmount { get; set; }
		public decimal AlertAtAmount { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime? DateOfLastAlert { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string MaterialId { get; set; }
		public List<ContainerMaterial> Materials { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string HubId { get; set; }

		public bool IsDeleted { get; set; }
		public DateTime? DateDeleted { get; set; }
	}

	public class ContainerMaterial
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
	}
}