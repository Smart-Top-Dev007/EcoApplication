using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Commands.Scheduler;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Commands;
using EcoCentre.Models.Domain.Materials;
using Magnum.Linq;
using WebGrease.Css.Extensions;

namespace EcoCentre.Models.Domain.Reporting.Materials
{
	using Queries;

    public class UpdateMaterialsBroughtTask :Task<BackgroundTaskData>
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Material> _materialsRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<MaterialBrought> _materialBroughtRepository;

        // Cache repos
        private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
        private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;

        public UpdateMaterialsBroughtTask(TaskRepository taskRepository, Repository<Invoice> invoiceRepository, Repository<Material> materialsRepository,
            Repository<Client> clientRepository, Repository<MaterialBrought> materialBroughtRepository, BgTaskInterval interval,
            Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle, Repository<MaterialByAddress> cachedMaterialsByAddress)
            : base(taskRepository, interval)
        {
            _invoiceRepository = invoiceRepository;
            _materialsRepository = materialsRepository;
            _clientRepository = clientRepository;
            _materialBroughtRepository = materialBroughtRepository;

            _cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
            _cachedMaterialsByAddress = cachedMaterialsByAddress;
        }

        protected override void DoWork(DateTime execTime)
        {
            var invoices = _invoiceRepository.Query.Where(x => x.CreatedAt <= execTime && x.CreatedAt > Data.LastRun).ToList();
            if (invoices.Count < 1) return;
            if (null == Data || Data.LastRun.Ticks == 0)
            {
                _materialBroughtRepository.DropCollection();
            }

            var matIds = invoices.SelectMany(x => x.Materials).Select(x => x.MaterialId).Distinct().ToArray();
            var materials = _materialsRepository.Query.Where(x => matIds.Contains(x.Id)).ToList();
            var clientIds = invoices.Select(x => x.ClientId).Distinct().ToArray();
            var clients = _clientRepository.Query.Where(x => clientIds.Contains(x.Id)).ToList();
            var newRows = new List<MaterialBrought>();
            foreach (var invoice in invoices)
            {
                var client =clients.FirstOrDefault(x => x.Id == invoice.ClientId);
                if (null == client)
                {
                    continue;
                }
                var isExcludedInvoice = true;
                foreach (var material in invoice.Materials)
                {
                    var materialDetails = materials.FirstOrDefault(x => x.Id == material.MaterialId);
                    if (materialDetails == null) continue;
                    isExcludedInvoice &= materialDetails.IsExcluded;
                }

                foreach (var material in invoice.Materials)
                {
                    var materialDetails = materials.FirstOrDefault(x => x.Id == material.MaterialId);
                    if(materialDetails == null) continue;
                    var row = new MaterialBrought
                    {
                        InvoiceId = invoice.Id,
                        InvoiceNumber = invoice.InvoiceNo,
                        Center = invoice.Center,
                        MaterialName = materialDetails.Name,
                        MaterialNameLower = materialDetails.NameLower,
                        Amount = material.Quantity,
                        ClientId = invoice.ClientId,
                        CityId = client.Address.CityId,
                        CityName = client.Address.City,
                        Date = invoice.CreatedAt,
                        MaterialId = material.MaterialId,
                        ClientCategory = client.Category,
                        Unit = materialDetails.Unit,
                        IsExcludedInvoice = isExcludedInvoice,
						AmountPaid = material.Amount
					};
                    newRows.Add(row);
                }
            }
            // Drop the caches
            _cachedMaterialsByAddress.DropCollection();
            _cachedMaterialsByAddressBundle.DropCollection();
            
            // Save in batches - otherwise max mongo document size can be reached.
	        foreach (var batch in newRows.Batch(1000))
	        {
		        _materialBroughtRepository.InsertBatch(batch.ToList());
	        }
			
        }
    }
}