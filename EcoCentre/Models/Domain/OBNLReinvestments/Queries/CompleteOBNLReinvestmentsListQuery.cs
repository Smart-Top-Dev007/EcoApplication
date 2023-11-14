using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class CompleteOBNLReinvestmentsListQuery
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;

        public CompleteOBNLReinvestmentsListQuery(Repository<OBNLReinvestment> invoiceRepository, 
            Repository<Client> clientRepository, 
            Repository<Material> materialRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
        }

        public IList<OBNLReinvestmentDetails> Execute(CompleteOBNLReinvestmentsListQueryParams @param)
        {
            var thisYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

            var invoicesQuery = _invoiceRepository.Query;
            var curClient = param.Client ?? _clientRepository.FindOne(param.ClientId);

            //var clientsQuery = _clientRepository.Query.Where(x =>
            //    x.Address.City.Contains(curClient.Address.City) &&
            //    x.Address.StreetLower.Contains(curClient.Address.StreetLower) &&
            //    x.Address.StartsWith.Contains(curClient.Address.CivicNumber) &&
            //    x.Address.PostalCode.Contains(curClient.Address.PostalCode));
            //var clientsIds = clientsQuery.Select(x => x.Id).ToList();

            invoicesQuery = invoicesQuery.Where(x => x.ClientId == curClient.Id); // clientsIds.Contains(x.ClientId));

            invoicesQuery = invoicesQuery.Where(x => x.CreatedAt >= thisYear);
            invoicesQuery = invoicesQuery.OrderBy(x => x.CreatedAt);

            //var clients = clientsQuery.ToList();
            var invoices = invoicesQuery.ToList();
            var invoiceDetailsList = invoices.Select(i =>
            {
                var result = new OBNLReinvestmentDetails(i, curClient);
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
