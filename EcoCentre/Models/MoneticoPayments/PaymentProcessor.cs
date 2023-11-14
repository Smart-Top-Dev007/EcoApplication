using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EcoCentre.Models.MoneticoPayments
{
	public class PaymentProcessor
	{
		public Dictionary<string, string> CreateRedirect(PaymentRequest request, MoneticoConfiguration configuration)
		{
			var fields = CreatePaymentFields(request, configuration);
			var secretKey = GetKey(configuration);
			var mac = CalculateMac(fields, secretKey);
			return GetRedirectValues(fields, mac);
		}
		
		private PaymentFields CreatePaymentFields(PaymentRequest request, MoneticoConfiguration configuration)
		{
			return new PaymentFields
			{
				Version = "3.0",
				Tpe = configuration.Tpe,
				Date = request.Date.ToString("dd'/'MM'/'yyyy:HH:mm:ss"),
				Amount = request.Amount.ToString(CultureInfo.InvariantCulture) + configuration.Currency,
				Reference = request.Reference,
				FreeText = request.FreeText,
				Language = configuration.Language,
				Company = configuration.Company,
				Email = request.Email,
				RedirectUrl = request.RedirectUrl,
				RedirectSuccessUrl = request.RedirectSuccessUrl,
				RedirectErrorUrl = request.RedirectErrorUrl,
			};
		}
		
		private Dictionary<string, string> GetRedirectValues(PaymentFields fields, string mac)
		{
			var result = new Dictionary<string, string>
			{
				["version"] = fields.Version,
				["TPE"] = fields.Tpe,
				["date"] = fields.Date,
				["montant"] = fields.Amount,
				["reference"] = fields.Reference,
				["texte-libre"] = fields.FreeText,
				["lgue"] = fields.Language,
				["societe"] = fields.Company,
				["mail"] = fields.Email,
				["MAC"] = mac,
				["url_retour"] = fields.RedirectUrl,
				["url_retour_ok"] = fields.RedirectSuccessUrl,
				["url_retour_err"] = fields.RedirectErrorUrl,
			};

			return result;
		}

		private byte[] GetKey(MoneticoConfiguration configuration)
		{
			var key = configuration.Key;
			return Enumerable.Range(0, key.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(key.Substring(x, 2), 16))
				.ToArray();
		}

		public static string CalculateMac(PaymentFields fields, byte[] secretKey)
		{
			var message = $"{fields.Tpe}*{fields.Date}*{fields.Amount}*{fields.Reference}*{fields.FreeText}*" +
			              $"{fields.Version}*{fields.Language}*{fields.Company}*{fields.Email}**********";

			using (var hmac = new HMACSHA1(secretKey))
			{
				return hmac.ComputeHash(Encoding.ASCII.GetBytes(message)).ToHex();
			}
		}
	}
}