using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Containers.Consumers
{
	public class RemoveFromContainerOnInvoiceDeleted : Consumes<InvoiceDeletedEvent>.All
	{
		private readonly Repository<Container> _containerRepository;
		private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
		
		public RemoveFromContainerOnInvoiceDeleted(
			Repository<DeletedInvoice> deletedInvoiceRepository,
			Repository<Container> containerRepository
		)
		{
			_deletedInvoiceRepository = deletedInvoiceRepository;
			_containerRepository = containerRepository;
		}

		public void Consume(InvoiceDeletedEvent message)
		{
			var invoice = _deletedInvoiceRepository.Query.FirstOrDefault(x => x.Invoice.Id == message.InvoiceId)?.Invoice;

			if (invoice?.Materials == null)
			{
				return;
			}

			foreach (var material in invoice.Materials.Where(x => !string.IsNullOrWhiteSpace(x.Container)))
			{
				var container = _containerRepository.FindOne(material.Container);
				if (container.IsDeleted)
				{
					continue;
				}
				container.FillAmount -= material.Quantity;
				if (container.FillAmount < 0)
				{
					container.FillAmount = 0;
				}
				_containerRepository.Save(container);
			}
		}
	}
}