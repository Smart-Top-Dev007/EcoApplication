using System;
using EcoCentre.Models.Domain.OBNLReinvestments.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class DeleteOBNLReinvestmentCommand
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<DeletedOBNLReinvestment> _deletedInvoiceRepository;
    	private readonly IServiceBus _serviceBus;

        private readonly TaskRepository _taskRepository;

        public DeleteOBNLReinvestmentCommand(Repository<OBNLReinvestment> invoiceRepository, 
            Repository<DeletedOBNLReinvestment> deletedInvoiceRepository, 
            IServiceBus serviceBus, 
            TaskRepository taskRepository)
        {
            _invoiceRepository = invoiceRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;
        	_serviceBus = serviceBus;

            _taskRepository = taskRepository;
        }

        public DeletedOBNLReinvestment Execute(string id)
        {
            var invoice = _invoiceRepository.FindOne(id);
            var deleted = new DeletedOBNLReinvestment
            {
                OBNLReinvestment = invoice,
                    DeletedAt = DateTime.UtcNow
                };
            _invoiceRepository.Remove(invoice);
            _deletedInvoiceRepository.Insert(deleted);
        	_serviceBus.Publish(new OBNLReinvestmentDeletedEvent
            {
        	                    		InvoiceId = invoice.Id
        	                    	});

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsLimitExceededTask"));

            return deleted;
        }
    }
}