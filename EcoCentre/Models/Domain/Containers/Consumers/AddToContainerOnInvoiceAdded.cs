using System;
using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.Containers.Consumers
{
	public class AddToContainerOnInvoiceAdded : Consumes<InvoiceAddedEvent>.All
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<Container> _containerRepository;
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly SendAlertCommand _sendAlertCommand;

		public AddToContainerOnInvoiceAdded(
			Repository<Invoice> invoiceRepository,
			Repository<Container> containerRepository,
			SendAlertCommand sendAlertCommand
		)
		{
			_invoiceRepository = invoiceRepository;
			_containerRepository = containerRepository;
			_sendAlertCommand = sendAlertCommand;
		}

		public void Consume(InvoiceAddedEvent message)
		{
			var invoice = _invoiceRepository.FindOne(message.InvoiceId);


			if (invoice.Materials == null)
			{
				return;
			}

			foreach (var material in invoice.Materials.Where(x => !string.IsNullOrWhiteSpace(x.Container)))
			{
				var container = _containerRepository.FindOne(material.Container);
				container.FillAmount += material.Quantity;
				_containerRepository.Save(container);
				var shouldSendAlertEmail = container.FillAmount >= container.AlertAtAmount
						&& container.AlertAtAmount > 0
						&& container.DateOfLastAlert == null;

				if (shouldSendAlertEmail)
				{
					try
					{
						_sendAlertCommand.Execute(container.Id, invoice.CreatedBy?.UserId);
					}
					catch (Exception ex)
					{
						Logger.Error(ex);
					}
				}
			}

		}
	}
}
