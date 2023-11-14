using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Payments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
    public class InvoiceJournal : Entity
    {
        public InvoiceJournal()
        {
            Materials = new List<InvoiceJournalMaterial>();
        }
        public string InvoiceNo { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string InvoiceId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CityId { get; set; }

        public List<InvoiceJournalMaterial> Materials { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string HubId { get; set; }
        public string Type { get; set; }
        public string Street { get; set; }
        public string CivicNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
	    public decimal AmountIncludingTaxes { get; set; }
	    public string AptNumber { get; set; }
    }
}