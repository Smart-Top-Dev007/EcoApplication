using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.OBNLReinvestments;
using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Reporting.OBNL;

namespace EcoCentre.Models.Queries
{
    public class OBNLGlobalReportQuery
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;
        private readonly Repository<OBNLReinvestment> _obnlReinvestmentRepository;

        public OBNLGlobalReportQuery(Repository<Client> clientRepository,
            Repository<Material> materialRepository,
            Repository<OBNLReinvestment> obnlReinvestmentRepository)
        {
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
            _obnlReinvestmentRepository = obnlReinvestmentRepository;
        }

        public List<OBNLGlobal> ExecuteAll(OBNLGlobalReportParam param)
        {
            var materials = _materialRepository.Query
                .Where(x => x.Active)
                .ToList();

            //Fix when migrate to new mongo driver, that supports nested conditions
            var clients = _clientRepository.Query.Where(c => c.Category == "OBNL").ToList();
            if (!string.IsNullOrWhiteSpace(param.OBNLNumber))
            {
                clients = clients
                    .Where(x => x.OBNLNumbers
                    .Any(xx => xx.Equals(param.OBNLNumber, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            var result = new List<OBNLGlobal>();
            foreach (var client in clients)
            {
                var reinvestments = _obnlReinvestmentRepository.Query.Where(i => i.ClientId == client.Id);
                if (!string.IsNullOrWhiteSpace(param.CenterName) &&
                    !param.CenterName.Equals("Tous", StringComparison.OrdinalIgnoreCase))
                {
                    reinvestments = reinvestments.Where(x => x.Center.Name == param.CenterName);
                }
                if (param.FromDate.HasValue)
                {
                    reinvestments = reinvestments.Where(x => x.CreatedAt >= param.FromDate.Value);
                }
                if (param.ToDate.HasValue)
                {
                    reinvestments = reinvestments.Where(x => x.CreatedAt <= param.ToDate.Value);
                }

                var materialsMap = new Dictionary<string, double>(materials.Count);
                materials.ForEach(x => materialsMap.Add(x.Id, 0));
                foreach (var obnlReinvestment in reinvestments)
                {
                    foreach(var material in obnlReinvestment.Materials)
                    {
                        materialsMap[material.MaterialId] += material.Weight;
                    }                    
                }

                result.Add(new OBNLGlobal
                {
                    ClientId = client.Id,
                    Name = client.FirstName + client.LastName,
                    City = client.Address.City,
                    PostalCode = client.Address.PostalCode,
                    Address = client.Address.Street,
                    OBNLReinvestments = reinvestments,
                    IncludedOBNLReinvestments = reinvestments,
                    Materials = materialsMap.Select(x => new OBNLGlobalMaterial
                    {
                        Name = materials.Single(xx => xx.Id == x.Key).Name,
                        Weight = x.Value
                    })
                });
            }

            return result;
        }
    }
}