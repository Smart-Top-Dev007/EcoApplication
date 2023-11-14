using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Limits;

namespace EcoCentre.Models.Queries
{
	public class LimitsDetailsQueryResult
	{
		public LimitStatus Limit { get; set; }
		public IList<Client> Clients { get; set; }
	}
}