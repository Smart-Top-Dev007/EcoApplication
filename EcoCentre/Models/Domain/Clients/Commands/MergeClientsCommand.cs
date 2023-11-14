using System;
using EcoCentre.Models.Domain.Municipalities;
using EcoCentre.Models.ViewModel;
using FluentValidation;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain.Invoices;
using System.Linq;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Queries;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Reporting.Journal;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    using GlobalSettings.Queries;

    public class MergeClientsCommand
    {
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly CompleteInvoicesListQuery _completeInvoicesListQuery;

        private readonly Repository<MaterialBrought> _materialBroughtRepository;
        private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
        private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;

        private readonly Repository<InvoiceJournal> _journalRepository;

        private readonly TaskRepository _taskRepository;
        private readonly Repository<ClientAddress> _clientAddressRepository;

        public MergeClientsCommand(GlobalSettingsQuery globalSettingsQuery, Repository<Client> clientRepository, Repository<ClientAddress> clientAddressRepository, 
            Repository<Invoice> invoiceRepository, CompleteInvoicesListQuery completeInvoicesListQuery,
            Repository<MaterialBrought> materialBroughtRepository, Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle,
            Repository<MaterialByAddress> cachedMaterialsByAddress, Repository<InvoiceJournal> journalRepository, TaskRepository taskRepository)
        {
            _clientRepository = clientRepository;
            _clientAddressRepository = clientAddressRepository;
            _invoiceRepository = invoiceRepository;
            _completeInvoicesListQuery = completeInvoicesListQuery;

            _materialBroughtRepository = materialBroughtRepository;
            _cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
            _cachedMaterialsByAddress = cachedMaterialsByAddress;

            _journalRepository = journalRepository;
            
            _taskRepository = taskRepository;
            _globalSettingsQuery = globalSettingsQuery;
        }

        public  ClientViewModel Execute(MergeCommandParams @params)
        {
            // all cached reports must be dropped
            _cachedMaterialsByAddressBundle.DropCollection();
            _cachedMaterialsByAddress.DropCollection();

            var globalSettings = _globalSettingsQuery.Execute();

            var mergeDestId = @params.MergeDest;
            var mergeDestClient = _clientRepository.FindOne(mergeDestId);

            var mergeSources = @params.MergeSourcesStr.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            int maximalPersonalLimit = mergeDestClient.PersonalVisitsLimit != null ? (int)mergeDestClient.PersonalVisitsLimit : 0;

            foreach (var mergeSourceId in mergeSources)
            {
                var mergeSourceClient = _clientRepository.FindOne(mergeSourceId);
	            if (mergeSourceClient == null)
	            {
		            throw new Exception($"Client {mergeSourceId} not found");
	            }
                var mergeSourceAddress = _clientAddressRepository.FindOne(mergeSourceClient.Address.Id);

                if (null != mergeSourceClient.PersonalVisitsLimit)
                {
                    maximalPersonalLimit = Math.Max(maximalPersonalLimit, (int)mergeSourceClient.PersonalVisitsLimit);
                }
                var invoicesToModifyList = _invoiceRepository.Query.Where(x => x.ClientId == mergeSourceId).ToList(); // it is way faster to work in memory
                foreach (var invoice in invoicesToModifyList)
                {
                    invoice.ClientId = mergeDestId;
                    invoice.Address = mergeDestClient.Address;
                    
                    _invoiceRepository.Save(invoice);

                    var journalRecordsToModifyList = _journalRepository.Query.Where(x => x.InvoiceId == invoice.Id).ToList();
                    foreach (var journalRecord in journalRecordsToModifyList)
                    {
                        journalRecord.City = mergeDestClient.Address.City;
                        journalRecord.CityId = mergeDestClient.Address.CityId;
                        journalRecord.CivicNumber = mergeDestClient.Address.CivicNumber;
                        journalRecord.AptNumber = mergeDestClient.Address.AptNumber;
                        journalRecord.ClientFirstName = mergeDestClient.FirstName;
                        journalRecord.ClientLastName = mergeDestClient.LastName;
                        journalRecord.ClientId = mergeDestClient.Id;
                        journalRecord.HubId = mergeDestClient.Hub != null ? mergeDestClient.Hub.Id : null;
                        journalRecord.PostalCode = mergeDestClient.Address.PostalCode;
                        journalRecord.Street = mergeDestClient.Address.Street;
                        journalRecord.Type = mergeDestClient.Category;

                        _journalRepository.Save(journalRecord);
                    }
                }

                var materialsBroughtToModifyList = _materialBroughtRepository.Query.Where(x => x.ClientId == mergeSourceId).ToList();
                foreach (var matBrought in materialsBroughtToModifyList)
                {
                    matBrought.ClientId = mergeDestId;
                    matBrought.ClientCategory = mergeDestClient.Category;

                    _materialBroughtRepository.Save(matBrought);
                }

                _clientRepository.Remove(mergeSourceClient);
                _clientAddressRepository.Remove(mergeSourceAddress);
            }

            mergeDestClient.SetPersonalVisitsLimit(maximalPersonalLimit);

            var thisYearDate = new DateTime(DateTime.Today.Year, 1, 1);
            var visitsCount = _invoiceRepository.Query.Count(x => x.CreatedAt > thisYearDate && x.ClientId == mergeDestClient.Id);

            mergeDestClient.VisitsLimitExceeded = (globalSettings.MaxYearlyClientVisits > 0 && (mergeDestClient.PersonalVisitsLimit == null || mergeDestClient.PersonalVisitsLimit == 0) &&
                                       visitsCount > globalSettings.MaxYearlyClientVisits) ||
                                      (mergeDestClient.PersonalVisitsLimit != null && mergeDestClient.PersonalVisitsLimit > 0 &&
                                       visitsCount > mergeDestClient.PersonalVisitsLimit);

            mergeDestClient.Verify();
            _clientRepository.Save(mergeDestClient);

            _materialBroughtRepository.ReIndex();
            _journalRepository.ReIndex();

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateMaterialsBroughtTask"));

            return new ClientViewModel(mergeDestClient, _completeInvoicesListQuery.Execute(new CompleteInvoicesListQueryParams { Client = mergeDestClient }));
        }

    }
}