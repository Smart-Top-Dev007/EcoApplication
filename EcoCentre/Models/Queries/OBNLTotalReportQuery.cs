using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients.Queries;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.OBNLReinvestments;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Domain.Reporting.OBNL;

namespace EcoCentre.Models.Queries
{
	// ReSharper disable once InconsistentNaming
    public class OBNLTotalReportQuery
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<OBNLReinvestment> _obnlReinvestmentRepository;

        public OBNLTotalReportQuery(Repository<Client> clientRepository,
            Repository<OBNLReinvestment> obnlReinvestmentRepository)
        {
            _clientRepository = clientRepository;
            _obnlReinvestmentRepository = obnlReinvestmentRepository;
        }

        public IEnumerable<OBNLTotal> Execute(OBNLTotalReportParam param)
        {
            List<OBNLTotal> result = new List<OBNLTotal>();
            //Fix when migrate to new mongo driver, that supports nested conditions
            var clients = _clientRepository.Query.Where(c => c.Category == "OBNL").ToList(); // clients with OBNLs
            if (!string.IsNullOrWhiteSpace(param.OBNLNumber))
            {
                clients = clients
                    .Where(x => x.OBNLNumbers
                    .Any(xx => xx.Equals(param.OBNLNumber, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
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

                double totalWeight = 0;
                foreach (var obnlReinvestment in reinvestments)
                {
                    totalWeight += obnlReinvestment.Materials.Sum(i => i.Weight);
                }

                var obnlReinvestments = new List<MaterialByAddressInvoice>();
                foreach (var obnlReinvestment in reinvestments)
                {
                    var resultInvoice = new MaterialByAddressInvoice
                    {
                        Id = obnlReinvestment.Id,
                        CenterName = obnlReinvestment.Center != null ? obnlReinvestment.Center.Name : "",
                        CenterUrl = obnlReinvestment.Center != null ? obnlReinvestment.Center.Url : "",
                        InvoiceNo = obnlReinvestment.OBNLReinvestmentNo
                    };
                    obnlReinvestments.Add(resultInvoice);
                }
                DateTime? lastVisitDate = null;
                if (reinvestments.Any())
                {
                    lastVisitDate = reinvestments.Where(i => i.ClientId == client.Id)
                        .OrderByDescending(i => i.CreatedAt)
                        .First()
                        .CreatedAt;
                }
                var resultRow = new OBNLTotal
                {
                    ClientId = client.Id,
                    OBNLNumber = string.Join(", ", client.OBNLNumber),
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    TotalWeight = totalWeight,
                    City = client.Address.City,
                    PostalCode = client.Address.PostalCode,
                    Address = client.Address.Street,
                    LastVisitDate = lastVisitDate,
                    LastVisit = lastVisitDate?.ToString("yyyy-MM-dd"),
                    TotalVisits = reinvestments.Count(),
                    FullName = client.FirstName + " " + client.LastName,
                    Invoices = obnlReinvestments
                };
                result.Add(resultRow);
            }

            if (result.Count != 0)
            {
                var sortReverse = param.SortDir == SortDir.Desc;
                Func<OBNLTotal, OBNLTotal, int> sortExpression;
                switch (param.SortBy)
                {
                    case OBNLTotalReportOrderBy.FullName:
                        sortExpression = (x, y) => x.FullName.CompareTo(y.FullName);
                        break;
                    case OBNLTotalReportOrderBy.OBNLNumber:
                        sortExpression = (x, y) => x.OBNLNumber.CompareTo(y.OBNLNumber);
                        break;
                    case OBNLTotalReportOrderBy.TotalVisits:
                        sortExpression = (x, y) => x.TotalVisits.CompareTo(y.TotalVisits);
                        break;
                    case OBNLTotalReportOrderBy.City:
                        sortExpression = (x, y) => x.City.CompareTo(y.City);
                        break;
                    case OBNLTotalReportOrderBy.Address:
                        sortExpression = (x, y) => x.Address.CompareTo(y.Address);
                        break;
                    case OBNLTotalReportOrderBy.PostalCode:
                        sortExpression = (x, y) => x.PostalCode.CompareTo(y.PostalCode);
                        break;
                    case OBNLTotalReportOrderBy.TotalWeight:
                        sortExpression = (x, y) => x.TotalWeight.CompareTo(y.TotalWeight);
                        break;
                    default:
                        sortExpression = (x, y) => String.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
                        break;
                }
                var comparer = new LambdaComparer<OBNLTotal>(sortExpression, sortReverse);
                result.Sort(comparer);
            }


            return result;
        }
    }
}