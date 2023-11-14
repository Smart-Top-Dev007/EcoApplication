using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Municipalities;

namespace EcoCentre.Models.Domain.Dashboard
{
    public class DashboardMunicipalitiesSummaryQuery
    {
        private readonly Repository<Municipality> _municipalityRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        public DashboardMunicipalitiesSummaryQuery(Repository<Municipality> municipalityRepository, Repository<Invoice> invoiceRepository)
        {
            _municipalityRepository = municipalityRepository;
            _invoiceRepository = invoiceRepository;
        }

        public IList<MunicipalityEntry> Execute(DashboardMunicipalitiesSummaryQueryParams param)
        {
            var municipalities = _municipalityRepository.Query.ToList();
            var query = _invoiceRepository.Query;
            if (param.From.HasValue)
                query = query.Where(x => x.CreatedAt >= param.From);
            if (param.To.HasValue)
                query = query.Where(x => x.CreatedAt < param.To.Value.AddDays(1));
            var invoices = query.ToList();

            var result = from municipality in municipalities
                         let municipalitySummary = CreateMunicipalitySummary(invoices, municipality)
                         where municipalitySummary.Visits > 0 || municipality.Enabled
                         select municipalitySummary;

            var orderedResult = result.OrderBy(x=>x.Name).ToList();
            var other = orderedResult.FirstOrDefault(x => x.Name == "Autre");
            if (other != null)
            {
                orderedResult.Remove(other);
                orderedResult.Add(other);
            }
            return orderedResult;

        }

        private static MunicipalityEntry CreateMunicipalitySummary(List<Invoice> invoices, Municipality municipality)
        {
            var municipalitySummary = new MunicipalityEntry
                {
                    Visits = invoices.Count(x => x.Address.CityId == municipality.Id),
                    Clients =
                        invoices.Where(x => x.Address.CityId == municipality.Id).Select(x => x.ClientId).Distinct().Count(),
                    Name = municipality.Name
                };
            return municipalitySummary;
        }
    }
}