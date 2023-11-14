using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Limits.Consumers;

namespace EcoCentre.Models.Domain.Limits.Tasks
{
	public class UpdateLimitsSummaryTask : AsyncAdminTask
	{
		private readonly Repository<Invoice> _invoiceRepository;
		private readonly Repository<LimitStatus> _limitStatusRepository;
		private readonly UpdateLimitsOnInvoiceAdded _consumer;

		public UpdateLimitsSummaryTask(Repository<Invoice> invoiceRepository, Repository<LimitStatus> limitStatusRepository,
			UpdateLimitsOnInvoiceAdded consumer)
		{
			_invoiceRepository = invoiceRepository;
			_limitStatusRepository = limitStatusRepository;
			_consumer = consumer;
		}

		protected override void DoWork()
		{
			var invoices = _invoiceRepository.Query.Select(x => x.Id).ToList();
			_limitStatusRepository.RemoveAll();
			var ic = invoices.Count;
			var ii = 0;
			foreach (var invoice in invoices)
			{
				ii++;
				_consumer.Consume(new InvoiceAddedEvent
				{
					InvoiceId = invoice
				});
				Progress = (decimal)ii / ic;
			}
		}
	}
}