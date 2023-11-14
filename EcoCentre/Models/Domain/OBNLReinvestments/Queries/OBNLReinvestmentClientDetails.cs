namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLReinvestmentClientDetails
    {
        public OBNLReinvestmentClientDetails(OBNLReinvestmentDetails invoice)
        {
            Id = invoice.Id;
            InvoiceNo = invoice.OBNLReinvestmentNo;
            Center = invoice.Center;
            IsExcluded = invoice.IsExcluded;
        }

        public CenterIdentification Center { get; set; }

        public string Id { get; set; }
        public string InvoiceNo { get; set; }

        public bool IsExcluded { get; set; }
    }
}