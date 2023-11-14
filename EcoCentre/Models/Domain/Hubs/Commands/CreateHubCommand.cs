namespace EcoCentre.Models.Domain.Hubs.Commands
{
    public class CreateHubCommand
    {
        private readonly Repository<Hub> _hubRepository;

        public CreateHubCommand(Repository<Hub> hubRepository)
        {
            _hubRepository = hubRepository;
        }

	    public string Name { get; set; }
	    public string InvoiceIdentifier { get; set; }
	    public decimal DefaultGiveawayPrice { get; set; }
	    public string Address { get; set; }
	    public string EmailForLoginAlerts { get; set; }
		public void Execute()
        {
            var hub = Hub.Create(Name);
	        hub.InvoiceIdentifier = InvoiceIdentifier;
	        hub.DefaultGiveawayPrice = DefaultGiveawayPrice;
	        hub.Address = Address;
	        hub.EmailForLoginAlerts = EmailForLoginAlerts;

			_hubRepository.Save(hub);
        }
    }
}