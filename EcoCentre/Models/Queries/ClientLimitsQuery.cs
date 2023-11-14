using System;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
    public class ClientLimitsQuery
    {
        private readonly Repository<Material> _materialRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<LimitStatus> _limitsRepository;

        public ClientLimitsQuery(Repository<Material> materialRepository, Repository<Client> clientRepository, Repository<Invoice> invoiceRepository, Repository<LimitStatus> limitsRepository)
        {
            _materialRepository = materialRepository;
            _clientRepository = clientRepository;
            _invoiceRepository = invoiceRepository;
            _limitsRepository = limitsRepository;
        }

        public MaterialLimitResult Execute(string id)
        {
            var theClient = _clientRepository.FindOne(id);
            if(theClient == null) 
                throw new Exception("client "+id+" not found");
	        var allClients = _clientRepository.Query
		        .Where(x => x.Address.StreetLower == theClient.Address.StreetLower &&
		                    x.Address.PostalCode == theClient.Address.PostalCode &&
		                    x.Address.CityId == theClient.Address.CityId &&
		                    x.Address.AptNumber == theClient.Address.AptNumber &&
		                    x.Address.CivicNumber == theClient.Address.CivicNumber)
		        .Select(x => x.Id)
		        .ToList();

            var thisYear = DateTime.UtcNow;
            thisYear = new DateTime(thisYear.Year,1,1);
            var allMaterials =
                _invoiceRepository.Query.Where(x => allClients.Contains(x.ClientId) && x.CreatedAt >= thisYear)
                                  .Select(x => x.Materials).ToList().SelectMany(x=>x).ToArray();
            var materialIds = allMaterials.Select(x => x.MaterialId).Distinct().ToArray();
            var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id)).ToList();
            var result = materials.Select(material => new MaterialLimitResultRow(material)).ToList();
            foreach (var m in allMaterials)
            {
                var material = result.FirstOrDefault(x => x.Material.Id == m.MaterialId);
                if (material == null) continue;
                material.Quantity += m.Quantity;
            }
            return new MaterialLimitResult
                {
                    Limits = result
                };

        }
    }
}