using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain;

namespace EcoCentre.Models.Infrastructure.SystemSettings
{
	public class SystemSettingsQuery
	{
		private readonly Repository<SystemSettings> _repository;

		public SystemSettingsQuery(Repository<SystemSettings> repository)
		{
			_repository = repository;
		}

		public SystemSettings Execute()
		{
			var settings = _repository.Query.FirstOrDefault();
			if (settings == null)
			{
				settings = GetDefault();
				_repository.Save(settings);
			}
			return settings;
		}

		private SystemSettings GetDefault()
		{
			return new SystemSettings
			{
				IsObnlEnabled = true
			};
		}
	}
}