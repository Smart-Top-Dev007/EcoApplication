namespace EcoCentre.Models.Domain.Invoices.Commands
{
    public class InvoiceMaterialViewModel
    {
        public string Id { get; set; }
        public decimal Quantity { get; set; }
	    public string Container { get; set; }
	    public bool ProvidedProofOfResidence { get; set; }
    }
}