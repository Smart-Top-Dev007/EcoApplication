using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialByAddressMaterial// : Entity
    {
        public string Id { get; set; }
        public String Name { get; set; }
        public String Tag { get; set; }
        public String Unit { get; set; }
        public decimal Total { get; set; }
    }
}