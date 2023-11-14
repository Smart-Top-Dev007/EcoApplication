using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
    public class ClientLimitsReportQuery
    {
        private readonly Repository<LimitStatus> _limitStatusRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Material> _materialRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        public ClientLimitsReportQuery(Repository<LimitStatus> limitStatusRepository, Repository<Client> clientRepository,
            Repository<Material> materialRepository, Repository<Invoice> invoiceRepository)
        {
            _limitStatusRepository = limitStatusRepository;
            _clientRepository = clientRepository;
            _materialRepository = materialRepository;
            _invoiceRepository = invoiceRepository;
        }

        private const int PageSize = 20;
        public PagedCollection<ClientLimitsReportQueryResultRow> Execute(ClientLimitsReportQueryParams @params)
        {
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            if (skip < 0) skip = 0;

            var today = DateTime.Today;
			
	        var itemsQuery = _limitStatusRepository.Query
				.Where(x => x.Limits.Any(l => l.IsExceeding && l.Year >= today.Year))
                .OrderByDescending(x => x.UpdatedAt);

	        List<LimitStatus> items;
	        if (pageSize > 0)
	        {
		        items = itemsQuery.Skip(skip).Take(pageSize).ToList();
	        }
	        else
	        {
		        items = itemsQuery.ToList();
	        }

	        var count = itemsQuery.Count();
            var result = new List<ClientLimitsReportQueryResultRow>();

            var addressIds = items.Select(x => x.Address.Id);
            var beginingOfYear = today.AddDays(-1* today.DayOfYear + 1);
            var invoices = _invoiceRepository.Query
                .Where(x => x.CreatedAt >= beginingOfYear)
                .Where(x => addressIds.Contains(x.Address.Id))
				.OrderByDescending(x=>x.CreatedAt)
                .ToList();
            var clietns = _clientRepository.Query.Where(x => addressIds.Contains(x.Address.Id)).ToList();
            var materialsDetails = _materialRepository.Query.ToList();
            foreach (var item in items)
            {
            	var limits = item.Limits.Single(x => x.Year == today.Year).Materials
            		.Where(x => x.QuantitySoFar > x.MaxQuantity)
            		.Select(x =>
            		        new ClientLimitsReportQueryResultMaterial
            		        	{
            		        		Material = materialsDetails.SingleOrDefault(m => m.Id == x.MaterialId),
            		        		MaxQuantity = x.MaxQuantity,
            		        		QuantitySoFar = x.QuantitySoFar
            		        	}).ToList();
                var row = new ClientLimitsReportQueryResultRow
                    {
                        Address = item.Address,
                        Id = item.Id,
						Date = item.UpdatedAt,
                        Invoices = invoices.Where(x=>x.Address.Id == item.Address.Id).ToList(),
                        Clients = clietns.Where(x=>x.Address.Id == item.Address.Id).ToList(),
                        Limits = limits
                    };

                result.Add(row);
            }
            return new PagedCollection<ClientLimitsReportQueryResultRow>(result, PageSize, (int)count, @params.Page);
        }
    }
}