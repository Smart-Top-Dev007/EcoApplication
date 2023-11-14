using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Giveaway.Consumers
{
	public class RestoreGiveawayOnInvoiceDeleted : Consumes<InvoiceDeletedEvent>.All
	{
		private readonly Repository<GiveawayItem> _giveawayRepository;
		private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
		
		public RestoreGiveawayOnInvoiceDeleted(
			Repository<DeletedInvoice> deletedInvoiceRepository,
			Repository<GiveawayItem> giveawayRepository
		)
		{
			_deletedInvoiceRepository = deletedInvoiceRepository;
			_giveawayRepository = giveawayRepository;
		}

		public void Consume(InvoiceDeletedEvent message)
		{
			var invoice = _deletedInvoiceRepository.Query.FirstOrDefault(x => x.Invoice.Id == message.InvoiceId)?.Invoice;

			if (invoice?.GiveawayItems == null)
			{
				return;
			}

			foreach (var item in invoice.GiveawayItems)
			{
				var itemEntity = _giveawayRepository.FindOne(item.Id);
				itemEntity.IsGivenAway = false;
				_giveawayRepository.Save(itemEntity);
			}
		}
	}
}