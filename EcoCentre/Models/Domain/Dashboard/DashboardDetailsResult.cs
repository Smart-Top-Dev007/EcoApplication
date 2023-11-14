using System.Collections.Generic;
using EcoCentre.Models.Domain.Municipalities;

namespace EcoCentre.Models.Domain.Dashboard
{
    public class DashboardDetailsResult
    {
        public int InvoicesToday { get; set; }
        public int InvoicesThisMonth { get; set; }
        public int InvoicesThisYear { get; set; }
        public int ClientsToday { get; set; }
        public int ClientsThisMonth { get; set; }
        public int ClientsThisYear { get; set; }
        public int OBNLVisitsToday { get; set; }
        public int OBNLVisitsThisMonth { get; set; }
        public int OBNLVisitsThisYear { get; set; }
        public double WeightToday { get; set; }
        public double WeightThisMonth { get; set; }
        public double WeightThisYear { get; set; }
        public ChartData MonthLog { get; set; }
        public IList<EcoCenterEntry> EcoCentersSummary { get; set; }
        public long MaxExceeded { get; set; }
    }
}