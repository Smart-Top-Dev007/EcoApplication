using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Reporting.Materials;

namespace EcoCentre.Models.Queries
{
    public class MaterialsByAddressReportQueryResultRow
    {
        public String ClientId { get; set; }
        public String Name { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String CivicNumber { get; set; }
        public String PostalCode { get; set; }
        public int PersonalVisitsLimit { get; set; }

        public IList<MaterialByAddressInvoice> Invoices { get; set; }
        public IList<MaterialByAddressInvoice> IncludedInvoices { get; set; }
        public IList<MaterialByAddressInvoice> ExcludedInvoices { get; set; }

        public IList<MaterialByAddressMaterial> Materials { get; set; }
	    public decimal Amount { get; set; }
	    public decimal AmountIncludingTaxes { get; set; }
	    public string AptNumber { get; set; }
    }
}