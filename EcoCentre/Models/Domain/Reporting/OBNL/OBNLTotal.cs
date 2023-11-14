using EcoCentre.Models.Domain.Reporting.Materials;
using System;
using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Reporting.OBNL
{
    public class OBNLTotal
    {
        public string OBNLNumber { get; set; }
        public string ClientId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string LastVisit { get; set; }
        public double TotalWeight { get; set; }
        public int TotalVisits { get; set; }
        public IList<MaterialByAddressInvoice> Invoices { get; set; }
    }
}