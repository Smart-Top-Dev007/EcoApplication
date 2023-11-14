using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    using Materials;

    public class CompleteInvoicesListQuery
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;

        public CompleteInvoicesListQuery(Repository<Invoice> invoiceRepository, Repository<Client> clientRepository, Repository<Material> materialRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
        }

        public IList<InvoiceDetails> Execute(CompleteInvoicesListQueryParams @param)
        {
            var thisYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

            var invoicesQuery = _invoiceRepository.Collection.AsQueryable();
            var curClient = param.Client ?? _clientRepository.FindOne(param.ClientId);
			
            invoicesQuery = invoicesQuery.Where(x => x.ClientId == curClient.Id);

            invoicesQuery = invoicesQuery.Where(x => x.CreatedAt >= thisYear);
            invoicesQuery = invoicesQuery.OrderBy(x => x.CreatedAt);

            var invoices = invoicesQuery.ToList();
            var invoiceDetailsList = invoices.Select(i =>
            {
                var result = new InvoiceDetails(i, curClient);
                var materialIds = i.Materials.Select(x => x.MaterialId);
                var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id));

                result.IsExcluded = true;
                foreach (var material in i.Materials)
                {
                    if (material == null || material.MaterialId == null)
                    {
                        // damaged invoice cannot be exluded - no material info got saved for it anyways
                        result.IsExcluded = false;
                        continue;
                    }
                    result.IsExcluded &= materials.First(x => x.Id == material.MaterialId).IsExcluded;
                }
                return result;
            });//clients.FirstOrDefault(x => x.Id == i.ClientId)));

            return invoiceDetailsList.ToList();
        }
    }
}
