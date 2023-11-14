using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Queries
{
    using Domain.Invoices.Queries;

    public class ClientVisitsLimitsReportQueryResultRow
	{
		public string Id { get; set; }
		public ClientAddress Address { get; set; }
        public IList<ClientVisitsLimitsReportQueryResultMaterial> Limits { get; set; }
		public Client Client { get; set; }

		public IList<InvoiceDetails> Invoices { get; set; }

		public DateTime? Date { get; set; }
	}
}