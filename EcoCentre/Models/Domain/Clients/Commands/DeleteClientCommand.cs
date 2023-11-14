using System;
using EcoCentre.Models.Domain.Invoices;
using System.Linq;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Queries;
using EcoCentre.Models.Domain.Reporting.Journal;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    public class DeleteClientCommand
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        private readonly Repository<MaterialBrought> _materialBroughtRepository;
        private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
        private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;

        private readonly Repository<InvoiceJournal> _journalRepository;

        private readonly TaskRepository _taskRepository;
        private readonly Repository<ClientAddress> _clientAddressRepository;

        public DeleteClientCommand(Repository<Client> clientRepository, Repository<ClientAddress> clientAddressRepository, Repository<Invoice> invoiceRepository,
            Repository<MaterialBrought> materialBroughtRepository, Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle,
            Repository<MaterialByAddress> cachedMaterialsByAddress, Repository<InvoiceJournal> journalRepository, TaskRepository taskRepository)
        {
            _clientRepository = clientRepository;
            _clientAddressRepository = clientAddressRepository;
            _invoiceRepository = invoiceRepository;

            _materialBroughtRepository = materialBroughtRepository;
            _cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
            _cachedMaterialsByAddress = cachedMaterialsByAddress;

            _journalRepository = journalRepository;

            _taskRepository = taskRepository;
        }

        public void Execute(String id)
        {
            var client = _clientRepository.Query.SingleOrDefault(x => x.Id == id);
            if (client == null)
                throw new Exception("Client not found");

            var clientAddress = _clientAddressRepository.FindOne(client.Address.Id);

            var invoicesToDeleteList = _invoiceRepository.Query.Where(x => x.ClientId == client.Id).ToList(); // it is way faster to work in memory
            foreach (var invoice in invoicesToDeleteList)
            {

                var journalRecordsToModifyList = _journalRepository.Query.Where(x => x.InvoiceId == invoice.Id).ToList();
                foreach (var journalRecord in journalRecordsToModifyList)
                {
                    _journalRepository.Remove(journalRecord);
                }
                _invoiceRepository.Remove(invoice);
            }

            var materialsBroughtToModifyList = _materialBroughtRepository.Query.Where(x => x.ClientId == client.Id).ToList();
            foreach (var matBrought in materialsBroughtToModifyList)
            {
                _materialBroughtRepository.Remove(matBrought);
            }

            _clientRepository.Remove(client);
            _clientAddressRepository.Remove(clientAddress);

            // all cached reports must be dropped
	        var materialsBundleCollection = _cachedMaterialsByAddressBundle.Collection;
	        materialsBundleCollection.Database.DropCollection(materialsBundleCollection.CollectionNamespace.CollectionName);

            _cachedMaterialsByAddress.Collection.Database.DropCollection(_cachedMaterialsByAddress.Collection.CollectionNamespace.CollectionName);


			// todo: fix
			// _materialBroughtRepository.Collection.ReIndex();
            //_journalRepository.Collection.ReIndex();

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));
        }

    }
}