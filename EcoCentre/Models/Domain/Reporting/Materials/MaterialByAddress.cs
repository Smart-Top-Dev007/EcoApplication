using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    [BsonIgnoreExtraElements]
    public class MaterialByAddress : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }
        public string FirstName { get; set; }
        public string FirstNameLower { get; set; }
        public string LastName { get; set; }
        public string LastNameLower { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetLower { get; set; }
        public string CivicNumber { get; set; }
        public string PostalCode { get; set; }
        public int PersonalVisitsLimit { get; set; }

        public IList<MaterialByAddressInvoice> Invoices { get; set; }
        public IList<MaterialByAddressInvoice> IncludedInvoices { get; set; }
        public IList<MaterialByAddressInvoice> ExcludedInvoices { get; set; }
        public IList<MaterialByAddressMaterial> Materials { get; set; }
        public Int32 TotalInvoices { get; set; }
        public Int32 TotalExcludedInvoices { get; set; }
        public Int32 TotalIncludedInvoices { get; set; }
	    public decimal Amount { get; set; }
	    public decimal AmountIncludingTaxes { get; set; }
	    public string AptNumber { get; set; }
    }
}