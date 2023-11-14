using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.Limits.Queries
{
	public class LimitListQueryResult
	{
		private readonly LimitStatus _status;
		public ClientAddress Address
		{
			get { return _status.Address; }
			set { _status.Address = value; }
		}

		public string Id
		{
			get { return _status.Id; }
			set { _status.Id = value; }
		}

		public IList<LimitStatusYear> Limits
		{
			get { return _status.Limits; }
			set { _status.Limits = value; }
		}

		public IList<LimitStatusMaterial> CurrentLimits
		{
			get
			{
				var thisYear = _status.Limits.FirstOrDefault(x => x.Year == DateTime.UtcNow.Year);
				if (thisYear != null)
				{
					return thisYear.Materials.OrderBy(x => x.MaterialName).ToList();
				}

				return new List<LimitStatusMaterial>();
			}
		}

		public LimitListQueryResult(LimitStatus status)
		{
			_status = status;
		}
	}
}