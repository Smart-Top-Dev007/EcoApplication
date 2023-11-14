using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    [BsonIgnoreExtraElements]
    public class MaterialBrought : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public CenterIdentification Center { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameLower { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        public string ClientCategory { get; set; }
        public decimal Amount { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CityId { get; set; }
        public string CityName { get; set; }

        public DateTime Date { get; set; }

        public string Unit { get; set; }
        public bool IsExcludedInvoice { get; set; }
	    public decimal AmountPaid { get; set; }
	}
}