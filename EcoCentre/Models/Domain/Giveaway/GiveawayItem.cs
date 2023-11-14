using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class GiveawayItem :Entity
	{
		public int SequenceNo  { get; set; }
		public string Title { get; set; }
		public string TitleLower { get; set; }
		public string Description { get; set; }
		public string DescriptionLower { get; set; }
		public DateTime DateAdded { get; set; }
		public bool IsPublished { get; set; }
		public decimal Price { get; set; }
		public string Type { get; set; }
		public string TypeLower { get; set; }
		public string ImageId { get; set; }
		public bool IsDeleted { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string HubId { get; set; }
		public string HubName { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string CreatedByUserId { get; set; }
		public bool IsGivenAway { get; set; }
	}
}