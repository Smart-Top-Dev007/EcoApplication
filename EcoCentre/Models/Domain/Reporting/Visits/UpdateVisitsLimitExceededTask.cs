using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Commands.Scheduler;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
    using System.Diagnostics;
    using GlobalSettings.Queries;
    using Queries;

    public class UpdateVisitsLimitExceededTask :Task<BackgroundTaskData>
    {
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Material> _materialsRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<MaterialBrought> _materialBroughtRepository;

        // Cache repos
        private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
        private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;

        public UpdateVisitsLimitExceededTask(GlobalSettingsQuery globalSettingsQuery, TaskRepository taskRepository, Repository<Invoice> invoiceRepository, 
            Repository<Material> materialsRepository, Repository<Client> clientRepository, Repository<MaterialBrought> materialBroughtRepository, 
            BgTaskInterval interval, Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle, Repository<MaterialByAddress> cachedMaterialsByAddress)
            : base(taskRepository, interval)
        {
            _globalSettingsQuery = globalSettingsQuery;
            _invoiceRepository = invoiceRepository;
            _materialsRepository = materialsRepository;
            _clientRepository = clientRepository;
            _materialBroughtRepository = materialBroughtRepository;

            _cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
            _cachedMaterialsByAddress = cachedMaterialsByAddress;
        }

        protected override void DoWork(DateTime execTime)
        {
            var thisYearDate = new DateTime(DateTime.Today.Year, 1, 1);

            bool reset = null == Data || Data.LastRun.Ticks == 0 || Data.LastRun.Year < thisYearDate.Year;

            var globalSettings = _globalSettingsQuery.Execute();

            if (reset)
            {
                var allClients = _clientRepository.Query.Where(x => x.VisitsLimitExceeded);
                foreach (var client in allClients)
                {
                    client.VisitsLimitExceeded = false;
                    _clientRepository.Save(client);
                }
            }

            var invoices = reset
                ? _invoiceRepository.Query.Where(
                    x => x.CreatedAt <= execTime && x.CreatedAt > thisYearDate).OrderBy(x => x.CreatedAt).ToList()
                : _invoiceRepository.Query.Where(
                    x => x.CreatedAt <= execTime && x.CreatedAt > Data.LastRun).OrderBy(x => x.CreatedAt).ToList();

            List<Invoice> invoicesList = invoices.Where(i =>
            {
                var materialIds = i.Materials.Select(x => x.MaterialId);
                var materials = _materialsRepository.Query.Where(x => materialIds.Contains(x.Id));

                return i.Materials.Any(material => material == null || material.MaterialId == null || !materials.First(x => x.Id == material.MaterialId).IsExcluded);
            }).ToList();

            if (!invoicesList.Any()) return;

            var clientIds = invoicesList.Select(x => x.ClientId);
            var clients = _clientRepository.Query.Where(x => clientIds.Contains(x.Id));
            foreach (var client in clients)
            {
                var visitsCount = invoicesList.Count(x => x.ClientId == client.Id);
                var isLimitExceeded = (globalSettings.MaxYearlyClientVisits > 0 && (client.PersonalVisitsLimit == null || client.PersonalVisitsLimit == 0) &&
                                       visitsCount > globalSettings.MaxYearlyClientVisits) ||
                                      (client.PersonalVisitsLimit != null && client.PersonalVisitsLimit > 0 &&
                                       visitsCount > client.PersonalVisitsLimit);

                if (isLimitExceeded != client.VisitsLimitExceeded || reset)
                {
                    client.VisitsLimitExceeded = isLimitExceeded;
                    _clientRepository.Save(client);
                }
            }
        }
    }
}