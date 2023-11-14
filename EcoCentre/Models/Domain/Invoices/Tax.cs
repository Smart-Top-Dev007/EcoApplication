namespace EcoCentre.Models.Domain.Invoices
{
	public class Tax
	{
		public string Name { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
	}
}