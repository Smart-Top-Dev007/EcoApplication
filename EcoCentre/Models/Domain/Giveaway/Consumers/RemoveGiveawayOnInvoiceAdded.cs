using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Giveaway.Consumers
{
	public class RemoveGiveawayOnInvoiceAdded : Consumes<InvoiceAddedEvent>.All
	{
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<GiveawayItem> _giveawayRepository;

		public RemoveGiveawayOnInvoiceAdded(
			Repository<Invoice> invoiceRepository,
			Repository<GiveawayItem> giveawayRepository
		)
		{
			_invoiceRepository = invoiceRepository;
			_giveawayRepository = giveawayRepository;
		}

		public void Consume(InvoiceAddedEvent message)
		{
			var invoice = _invoiceRepository.FindOne(message.InvoiceId);

			if (invoice.GiveawayItems == null)
			{
				return;
			}

			foreach (var item in invoice.GiveawayItems)
			{
				var itemEntity = _giveawayRepository.FindOne(item.Id);
				itemEntity.IsGivenAway = true;
				_giveawayRepository.Save(itemEntity);
			}

		}
	}
}
