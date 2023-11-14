using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
    public class OBNLMaterialsQuery
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;

        public OBNLMaterialsQuery(Repository<OBNLReinvestment> invoiceRepository,
            Repository<Client> clientRepository,
            Repository<Material> materialRepository)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
        }

        public IList<OBNLMaterialRow> Execute(OBNLMaterialsQueryParams param)
        {
            var thisYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

            var curClient = _clientRepository.FindOne(param.ClientId);

            var obnlReinvestments = _invoiceRepository.Query.Where(x => x.ClientId == curClient.Id
                                                && x.CreatedAt >= thisYear)
                                            .OrderBy(x => x.CreatedAt).ToList();

            var materials = _materialRepository.Query.Where(x => x.Active).ToList();

            var materialsMap = new Dictionary<string, double>(materials.Count);
            materials.ForEach(x => materialsMap[x.Id] = 0);
            foreach (var obnlReinvestment in obnlReinvestments)
            {
                foreach (var material in obnlReinvestment.Materials)
                {
                    materialsMap[material.MaterialId] += material.Weight;
                }
            }

            return materialsMap.Select(x => 
                new OBNLMaterialRow(materials.Single(m => m.Id.Equals(x.Key)).Name, 
                                    x.Value))
                .ToList();
        }
    }
}