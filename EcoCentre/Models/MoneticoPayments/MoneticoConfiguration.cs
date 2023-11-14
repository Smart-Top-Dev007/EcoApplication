using System;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EcoCentre.Models.MoneticoPayments
{
	public class MoneticoConfiguration : Entity
	{
		public string Tpe { get; set; }
		public string Currency { get; set; }
		public string Language { get; set; }
		public string Company { get; set; }
		public string Key { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MoneticoEnvironment Environment { get; set; }

		public string RedirectUrl()
		{
			if (Environment == MoneticoEnvironment.Production)
			{
				return "https://p.monetico-services.com/paiement.cgi";
			}

			if(Environment == MoneticoEnvironment.Test)
			{
				return "https://p.monetico-services.com/test/paiement.cgi";
			}
			throw new Exception($"Unknown Monetico environment: {Environment}");
		}
	}
}