namespace EcoCentre.Models.Domain.Reporting.Materials
{
    public class MaterialBroughtQueryResultRow
    {
        public string Name { get; set; }
        public string MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public int Invoices { get; set; }
    }
}