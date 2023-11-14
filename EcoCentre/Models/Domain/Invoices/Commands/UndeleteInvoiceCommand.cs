using System.Linq;
using EcoCentre.Models.Domain.Invoices.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
    public class UndeleteInvoiceCommand
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
    	private readonly IServiceBus _serviceBus;

        private readonly TaskRepository _taskRepository;

    	public UndeleteInvoiceCommand(Repository<Invoice> invoiceRepository, Repository<DeletedInvoice> deletedInvoiceRepository,
            IServiceBus serviceBus, TaskRepository taskRepository)
        {
            _invoiceRepository = invoiceRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;
            _serviceBus = serviceBus;

            _taskRepository = taskRepository;
        }

        public Invoice Execute(string id)
        {
            var deleted = _deletedInvoiceRepository.Query.SingleOrDefault(x => x.Invoice.Id == id);
            if (deleted == null) return null;
            _invoiceRepository.Save(deleted.Invoice);
            _deletedInvoiceRepository.Remove(deleted);
			_serviceBus.Publish(new InvoiceAddedEvent
			{
			    InvoiceId = id
			});

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsNumberTask"));

            return deleted.Invoice;
        }
    }
}