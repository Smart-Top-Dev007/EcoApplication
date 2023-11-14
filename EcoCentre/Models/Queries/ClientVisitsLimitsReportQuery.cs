using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
	using Domain.Invoices.Queries;

    public class ClientVisitsLimitsReportQuery
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;
        private readonly Repository<Invoice> _invoiceRepository;


	    public ClientVisitsLimitsReportQuery(Repository<Client> clientRepository,
            Repository<Material> materialRepository, Repository<Invoice> invoiceRepository)
        {
	        _clientRepository = clientRepository;
            _materialRepository = materialRepository;
            _invoiceRepository = invoiceRepository;
        }

        private const int PageSize = 20;
        public PagedCollection<ClientVisitsLimitsReportQueryResultRow> Execute(ClientLimitsReportQueryParams @params)
        {
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            if (skip < 0) skip = 0;

            var clients = _clientRepository.Query.Where(x => x.VisitsLimitExceeded);
            
            if (pageSize > 0)
            {
                clients = clients.Skip(skip).Take(pageSize);
            }
            var clientsList = clients.ToList();

            var clientIds = clientsList.Select(x => x.Id);

            var thisYearDate = new DateTime(DateTime.Today.Year, 1, 1);
            var invoices = _invoiceRepository.Query.Where(x => clientIds.Contains(x.ClientId) && x.CreatedAt > thisYearDate).ToList();

            var count = clientsList.Count;
            var result = new List<ClientVisitsLimitsReportQueryResultRow>();

            foreach (var client in clients)
            {
                var curClientInvoices = invoices.Where(x => x.ClientId == client.Id).ToList();
                var lastInvoice = curClientInvoices.LastOrDefault();

                List<ClientVisitsLimitsReportQueryResultMaterial> curMaterials = new List<ClientVisitsLimitsReportQueryResultMaterial>();

                var invoiceDetailsList = curClientInvoices.Select(i =>
                {
                    var res = new InvoiceDetails(i, client);
                    var materialIds = i.Materials.Select(x => x.MaterialId);
                    var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id));

                    res.IsExcluded = true;
                    foreach (var material in i.Materials)
                    {
                        if (material?.MaterialId == null)
                        {
                            // damaged invoice cannot be exluded - no material info got saved for it anyways
                            res.IsExcluded = false;
                            continue;
                        }
                        res.IsExcluded &= materials.First(x => x.Id == material.MaterialId).IsExcluded;

                        var resultMaterial =
                            curMaterials.FirstOrDefault(x => x.Material.Id == material.MaterialId);
                        if (null == resultMaterial)
                        {
                            resultMaterial = new ClientVisitsLimitsReportQueryResultMaterial
                            {
                                Material = _materialRepository.Query.FirstOrDefault(x => x.Id == material.MaterialId),
                                QuantitySoFar = 0
                            };
                            curMaterials.Add(resultMaterial);
                        }
                        resultMaterial.QuantitySoFar += material.Quantity;
                    }
                    return res;
                }).ToList();

                result.Add(new ClientVisitsLimitsReportQueryResultRow
                {
                    Client = client,
                    Address = client.Address,
                    Date = lastInvoice?.CreatedAt,
                    Id = client.Id,
                    Invoices = invoiceDetailsList,
                    Limits = curMaterials
                });

            }

            return new PagedCollection<ClientVisitsLimitsReportQueryResultRow>(result, pageSize, count, @params.Page);
        }
    }
}