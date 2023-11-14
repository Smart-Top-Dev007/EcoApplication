namespace EcoCentre.Models.Domain.Hubs
{
    public class Hub : Entity
    {
        public string Name { get; set; }
        public string InvoiceIdentifier { get; set; }
        public decimal DefaultGiveawayPrice { get; set; }
	    public string Address { get; set; }
	    public string EmailForLoginAlerts { get; set; }

	    public static Hub Create(string name)
        {
            return new Hub
                {
                    Name = name
                };
        }
    }
}