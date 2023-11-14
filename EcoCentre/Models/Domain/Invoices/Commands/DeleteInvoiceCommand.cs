using System;
using System.Linq;
using EcoCentre.Models.Domain.Invoices.Events;
using EcoCentre.Models.Domain.Reporting.Materials;
using MassTransit;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
    public class DeleteInvoiceCommand
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
        private readonly Repository<MaterialBrought> _materialBroughtRepository;
    	private readonly IServiceBus _serviceBus;

        private readonly TaskRepository _taskRepository;

        public DeleteInvoiceCommand(Repository<Invoice> invoiceRepository, Repository<DeletedInvoice> deletedInvoiceRepository, Repository<MaterialBrought> materialBroughtRepository, IServiceBus serviceBus, TaskRepository taskRepository)
        {
            _invoiceRepository = invoiceRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;
            _materialBroughtRepository = materialBroughtRepository;
        	_serviceBus = serviceBus;

            _taskRepository = taskRepository;
        }

        public DeletedInvoice Execute(string id)
        {
            var invoice = _invoiceRepository.FindOne(id);
            var deleted = new DeletedInvoice
                {
                    Invoice = invoice,
                    DeletedAt = DateTime.UtcNow
                };
            _invoiceRepository.Remove(invoice);
            _deletedInvoiceRepository.Insert(deleted);
            var materialsBroughtToModifyList = _materialBroughtRepository.Query.Where(x => x.InvoiceId == id).ToList();
			foreach (var matBrought in materialsBroughtToModifyList)
			{
				_materialBroughtRepository.Remove(matBrought);
			}
        	_serviceBus.Publish(new InvoiceDeletedEvent
        	                    	{
        	                    		InvoiceId = invoice.Id
        	                    	});

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsLimitExceededTask"));

            return deleted;
        }
    }
}