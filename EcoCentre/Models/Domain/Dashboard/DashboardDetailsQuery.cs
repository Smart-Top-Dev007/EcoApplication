using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.OBNLReinvestments;

namespace EcoCentre.Models.Domain.Dashboard
{
    public class DashboardDetailsQuery
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<OBNLReinvestment> _obnlReinvestmentRepository;
        private readonly Repository<LimitStatus> _limitStatusRepository;
        private readonly DashboardEcoCenterSummaryQuery _dashboardEcoCenterSummaryQuery;

	    public DashboardDetailsQuery(
			Repository<Invoice> invoiceRepository,
            Repository<OBNLReinvestment> obnlReinvestmentRepository,
			Repository<LimitStatus> limitStatusRepository,
            DashboardEcoCenterSummaryQuery dashboardEcoCenterSummaryQuery)
        {
            _invoiceRepository = invoiceRepository;
            _obnlReinvestmentRepository = obnlReinvestmentRepository;
            _limitStatusRepository = limitStatusRepository;
            _dashboardEcoCenterSummaryQuery = dashboardEcoCenterSummaryQuery;
        }

        public DashboardDetailsResult Execute(DashboardEcoCenterSummaryQueryParams param)
        {
            var today = DateTime.UtcNow.Date;
            var thisMonth = today.AddDays(-1 * today.Day + 1);
            var thisYear = thisMonth.AddMonths(-1 * today.Month + 1);
            var result = new DashboardDetailsResult();

            result.InvoicesToday = _invoiceRepository.Query
                .Count(x => x.CreatedAt >= today);
            result.InvoicesThisMonth = _invoiceRepository.Query
                .Count(x => x.CreatedAt >= thisMonth);
            result.InvoicesThisYear = _invoiceRepository.Query
                .Count(x => x.CreatedAt >= thisYear);
            result.EcoCentersSummary = _dashboardEcoCenterSummaryQuery.Execute(param);
            result.ClientsToday = _invoiceRepository.Query
                .Where(x => x.CreatedAt >= today)
                .Select(x => x.ClientId).Distinct().Count();
            result.ClientsThisMonth = _invoiceRepository.Query
                .Where(x => x.CreatedAt >= thisMonth)
                .Select(x => x.ClientId).Distinct().Count();
            result.ClientsThisYear = _invoiceRepository.Query
                .Where(x => x.CreatedAt >= thisYear)
                .Select(x => x.ClientId).Distinct().Count();
	        
			var obnlReinvestmentsToday = _obnlReinvestmentRepository.Query
                .Where(x => x.CreatedAt >= today);
            result.OBNLVisitsToday = obnlReinvestmentsToday.Count();
            result.WeightToday = Math.Round(obnlReinvestmentsToday.Sum(x => x.Materials.Sum(xx => xx.Weight)), 2);

            var obnlReinvestmentsThisMonth = _obnlReinvestmentRepository.Query
              .Where(x => x.CreatedAt >= thisMonth);
            result.OBNLVisitsThisMonth = obnlReinvestmentsThisMonth.Count();
            result.WeightThisMonth = Math.Round(obnlReinvestmentsThisMonth.Sum(x => x.Materials.Sum(xx => xx.Weight)), 2);

            var obnlReinvestmentsThisYear = _obnlReinvestmentRepository.Query
              .Where(x => x.CreatedAt >= thisYear);
            result.OBNLVisitsThisYear = obnlReinvestmentsThisYear.Count();
            result.WeightThisYear = Math.Round(obnlReinvestmentsThisYear.Sum(x => x.Materials.Sum(xx => xx.Weight)), 2);

            var limitsCount = _limitStatusRepository.Query
				.Count(x => x.Limits.Any(l => l.IsExceeding && l.Year >= today.Year));
			
            result.MaxExceeded = limitsCount;

            var invoices =
                _invoiceRepository.Query.Where(x => x.CreatedAt >= thisMonth)
                                  .Select(x => new { x.ClientId, x.CreatedAt })
                                  .ToList();
            var date = thisMonth;
            var labels = new List<string>();
            var invoicesDS = new List<int>();
            var clientsDS = new List<int>();
            do
            {
                var invoicesForDay = invoices.Where(x => x.CreatedAt.Day == date.Day).ToList();
                labels.Add(date.Day.ToString());
                invoicesDS.Add(invoicesForDay.Count());
                clientsDS.Add(invoicesForDay.Select(x => x.ClientId).Distinct().Count());
                date = date.AddDays(1);
            } while (date.Month == thisMonth.Month);
            var log = new ChartData
            {
                labels = labels.ToArray(),
                datasets = new[]
                        {
                            new ChartDataSet
                                {
                                    fillColor = "rgba(220,220,220,0.5)",
                                    strokeColor = "rgba(220,220,220,1)",
                                    data = invoicesDS.ToArray()
                                },
                            new ChartDataSet
                                {
                                    fillColor = "rgba(151,187,205,0.5)",
                                    strokeColor = "rgba(151,187,205,1)",
                                    data = clientsDS.ToArray()
                                }
                        }
            };
            result.MonthLog = log;
            return result;
        }

    }
}