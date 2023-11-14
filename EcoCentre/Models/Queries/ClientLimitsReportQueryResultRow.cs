using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Queries
{
	public class ClientLimitsReportQueryResultRow
	{
		public string Id { get; set; }
		public ClientAddress Address { get; set; }
		public IList<ClientLimitsReportQueryResultMaterial> Limits { get; set; }
		public IList<Client> Clients { get; set; }

		public IList<Invoice> Invoices { get; set; }

		public DateTime? Date { get; set; }
	}
}