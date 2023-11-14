using System.Linq;
using EcoCentre.Models.Domain.OBNLReinvestments.Events;
using MassTransit;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class UndeleteOBNLReinvestmentCommand
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<DeletedOBNLReinvestment> _deletedInvoiceRepository;
    	private readonly IServiceBus _serviceBus;

        private readonly TaskRepository _taskRepository;

    	public UndeleteOBNLReinvestmentCommand(Repository<OBNLReinvestment> invoiceRepository, 
            Repository<DeletedOBNLReinvestment> deletedInvoiceRepository,
            IServiceBus serviceBus, TaskRepository taskRepository)
        {
            _invoiceRepository = invoiceRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;
            _serviceBus = serviceBus;

            _taskRepository = taskRepository;
        }

        public OBNLReinvestment Execute(string id)
        {
            var deleted = _deletedInvoiceRepository.Query.SingleOrDefault(x => x.OBNLReinvestment.Id == id);
            if (deleted == null) return null;
            _invoiceRepository.Save(deleted.OBNLReinvestment);
            _deletedInvoiceRepository.Remove(deleted);
			_serviceBus.Publish(new OBNLReinvestmentAddedEvent
            {
			    InvoiceId = id
			});

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsNumberTask"));

            return deleted.OBNLReinvestment;
        }
    }
}