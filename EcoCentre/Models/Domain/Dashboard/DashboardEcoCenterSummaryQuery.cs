using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.Dashboard
{
    public class DashboardEcoCenterSummaryQuery
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly CenterIdentification _centerIdentification;
        private readonly AuthenticationContext _authContext;
        private readonly Repository<Client> _clientRepository;
        public DashboardEcoCenterSummaryQuery(Repository<Invoice> invoiceRepository, CenterIdentification centerIdentification, AuthenticationContext authContext, Repository<Client> clientRepository)
        {
            _invoiceRepository = invoiceRepository;
            _centerIdentification = centerIdentification;
            _authContext = authContext;
            _clientRepository = clientRepository;
        }

        public IList<EcoCenterEntry> Execute(DashboardEcoCenterSummaryQueryParams param)
        {
            //var municipalities = _municipalityRepository.Query.ToList();
            var query = _invoiceRepository.Query;
            if (param.From.HasValue)
                query = query.Where(x => x.CreatedAt >= param.From);
            if (param.To.HasValue)
                query = query.Where(x => x.CreatedAt < param.To.Value.AddDays(1));

            if (!_authContext.User.IsGlobalAdmin)
            {
                query = query.Where(x => x.Center != null && x.Center.Name == _centerIdentification.Name);
            }

            var invoices = query.ToList();
            Dictionary<string, List<Invoice>> invoicesByCenter = new Dictionary<string, List<Invoice>>();
            foreach (var invoice in invoices)
            {
                string centerName = "-";
                if (null != invoice.Center && invoice.Center.Name != "")
                {
                    centerName = invoice.Center.Name;
                }
                if (!invoicesByCenter.ContainsKey(centerName))
                {
                    invoicesByCenter[centerName] = new List<Invoice>();
                }

                invoicesByCenter[centerName].Add(invoice);
            }

            List<Client> clientsObnl = _clientRepository.Query.Where(x => x.Category == "OBNL").ToList();

            var result = from key in invoicesByCenter.Keys
                         let ecoCenterSummary = CreateEcoCenterSummary(invoicesByCenter[key], key, clientsObnl)
                         select ecoCenterSummary;

            var orderedResult = result.OrderBy(x => x.Name).ToList();

            EcoCenterEntry totalEntry = new EcoCenterEntry();
            totalEntry.Name = "Total";

            foreach (var curResult in orderedResult)
            {
                totalEntry.Clients += curResult.Clients;
                totalEntry.Visits += curResult.Visits;
                totalEntry.OBNLVisits += curResult.OBNLVisits;
                totalEntry.OBNLWeight += curResult.OBNLWeight;
            }
            orderedResult.Insert(0, totalEntry);

            return orderedResult;
        }

        private static EcoCenterEntry CreateEcoCenterSummary(List<Invoice> invoices, string ecoCenterName, List<Client> clientsObnl)
        {
            var ecoCenterEntry = new EcoCenterEntry
            {
                Visits = invoices.Count,
                Clients =
                    invoices.Select(x => x.ClientId).Distinct().Count(),
                Name = ecoCenterName,
                OBNLVisits = 0,
                OBNLWeight = 0
            };
            foreach(var invoice in invoices)
            {
                if (clientsObnl.FirstOrDefault(x => x.Id == invoice.ClientId) == null) continue;
                else
                {
                    ecoCenterEntry.OBNLVisits++;
                    ecoCenterEntry.OBNLWeight += invoice.Materials.Sum(m => m.Weight);
                }
                

            }
            return ecoCenterEntry;
        }

    }
}