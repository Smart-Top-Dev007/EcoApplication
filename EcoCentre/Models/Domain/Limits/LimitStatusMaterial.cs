using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Limits
{
	public class LimitStatusMaterial
	{

		[BsonRepresentation(BsonType.ObjectId)]
		public string MaterialId { get; set; }
		public string MaterialName { get; set; }
		public decimal QuantitySoFar { get; set; }
		public decimal MaxQuantity { get; set; }

		public string Unit { get; set; }

		public bool IsActive { get; set; }
        public bool IsExcluded { get; set; }
	}
}