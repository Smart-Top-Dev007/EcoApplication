using System;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceDetailsMaterial
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public double Weight { get; set; }
        public string Unit { get; set; }
	    public decimal Price { get; set; }
	    public decimal Amount { get; set; }
	    public bool IsUsingFreeAmount { get; set; }
    }
}