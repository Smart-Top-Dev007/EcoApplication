﻿using System;
using System.Linq;

namespace EcoCentre.Models.Domain.Clients.Queries
{
	public class SuggestOBNLNumberQuery
	{
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<Client1> _clientRepository1;
		public SuggestOBNLNumberQuery(Repository<Client> clientRepository, Repository<Client1> clientRepository1)
		{
			_clientRepository = clientRepository;
			_clientRepository1 = clientRepository1;
		}

		public string[] Execute(string term, bool isCard = false)
		{
			if (isCard == false)
			{
				if (string.IsNullOrEmpty(term)) return new string[] { };

				var obnls = _clientRepository.Query
					.Where(x => x.Category == "OBNL")
					.ToList();

				return obnls.SelectMany(x => x.OBNLNumbers)
					.Where(x => x.StartsWith(term))
					.OrderBy(x => x)
					.Take(30)
					.Distinct()
					.ToArray();
			} else
			{
				return _clientRepository.Query.Where(x => x.CitizenCard.Contains(term))
										.OrderBy(x => x.CitizenCard)
										.Take(30)
										.Select(x => x.CitizenCard)
										.ToList()
										.Distinct().ToArray();
			}
			
		}

		public string[] Execute1(string term, bool isCard = false)
		{
			if (isCard == false)
			{
				if (string.IsNullOrEmpty(term)) return new string[] { };

				var obnls = _clientRepository1.Query
					.Where(x => x.Category == "OBNL")
					.ToList();

				return obnls.SelectMany(x => x.OBNLNumbers)
					.Where(x => x.StartsWith(term))
					.OrderBy(x => x)
					.Take(30)
					.Distinct()
					.ToArray();
			}
			else
			{
				return _clientRepository1.Query.Where(x => x.CitizenCard.Contains(term))
										.OrderBy(x => x.CitizenCard)
										.Take(30)
										.Select(x => x.CitizenCard)
										.ToList()
										.Distinct().ToArray();
			}

		}
	}
}