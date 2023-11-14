using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialByAddressInvoice// : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CenterName { get; set; }
        public string CenterUrl { get; set; }
        public string InvoiceNo { get; set; }
        public bool IsExcluded { get; set; }
    }
}