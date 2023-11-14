using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using EcoCentre.Models.Infrastructure;
using NLog;

namespace EcoCentre.Models.MoneticoPayments
{
	public class PaymentResponseValidator
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		public PaymentValidationResult ValidateResponse(string input, string key)
		{
			var fields = GetFields(input);
			var calculatedMac = CalculateMac(fields, key);
			var sentMac = fields["MAC"];

			if (sentMac != calculatedMac)
			{
				return new PaymentValidationResult
				{
					IsValid = false,
					Response = "version=2\ncdr=1\n"
				};
			}

			return new PaymentValidationResult
			{
				IsValid = true,
				Response = "version=2\ncdr=0\n",
				PaymentStatus = GetPaymentStatus(fields["code-retour"]),
				Reference = fields["reference"],
				CardType = GetCardType(fields.Get("brand")),
				Date = GetDate(fields.Get("date")),
				Amount = GetAmount(fields.Get("montant"))
			};			
		}

		private decimal GetAmount(string amount)
		{
			try
			{
				return decimal.Parse(amount.Replace("CAD", ""), NumberStyles.Any, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Could not amount from Monetico response. Attempted value: '{amount}' ");
				return 0;
			}
		}

		private DateTime GetDate(string date)
		{
			try
			{
				return DateTime.ParseExact(date.Replace("_a_", " "), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, $"Could not parse date from Monetico response. Attempted value: '{date}' ");
				return LocalDateTime.Now;
			}
		}

		private CardType GetCardType(string cardType)
		{
			switch (cardType)
			{
				case "VI": return CardType.Visa;
				case "AM": return CardType.Amex;
				case "MC": return CardType.MasterCard;
			}

			return CardType.NotAvailable;
		}

		private Dictionary<string, string> GetFields(string input)
		{
			var fields = HttpUtility.ParseQueryString(input);
			var result = new Dictionary<string,string>();
			foreach (var k in fields.AllKeys)
			{
				result.Add(k, fields[k]);
			}

			return result;
		}

		private PaymentStatus GetPaymentStatus(string returnCode)
		{
			switch (returnCode)
			{
				case "payetest": return PaymentStatus.Approved;
				case "paiement": return PaymentStatus.Approved;
				case "Annulation": return PaymentStatus.Declined;
			}

			throw new Exception($"Unknown return code {returnCode}");
		}

		private string CalculateMac(Dictionary<string, string> fields, string key)
		{
			var secretKey = Enumerable.Range(0, key.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(key.Substring(x, 2), 16))
				.ToArray();

			var f = fields;
			
			var message = $"{f.Get("TPE")}*" +
			              $"{f.Get("date")}*" +
			              $"{f.Get("montant")}*" +
			              $"{f.Get("reference")}*" +
			              $"{f.Get("texte-libre")}*" +
						  $"3.0*" +
						  $"{f.Get("code-retour")}*" +
			              $"{f.Get("cvx")}*" +
			              $"{f.Get("vld")}*" +
			              $"{f.Get("brand")}*" +
			              $"{f.Get("status3ds")}*" +
			              $"{f.Get("numauto")}*" +
			              $"{f.Get("motifrefus")}*" +
			              $"{f.Get("originecb")}*" +
			              $"{f.Get("bincb")}*" +
			              $"{f.Get("hpancb")}*" +
			              $"{f.Get("ipclient")}*" +
			              $"{f.Get("originetr")}*" +
			              $"{f.Get("veres")}*" +
			              $"{f.Get("pares")}*";

			using (var hmac = new HMACSHA1(secretKey))
			{
				return hmac.ComputeHash(Encoding.ASCII.GetBytes(message)).ToHex();
			}
		}
	}

	public enum CardType
	{
		NotAvailable = 1,
		Amex = 2,
		MasterCard = 3,
		Visa = 4
	}

	public class Payment
	{
		public string Reference { get; set; }
	}

	public class PaymentValidationResult
	{
		public string Response { get; set; }
		public bool IsValid { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public string Reference { get; set; }
		public CardType CardType { get; set; }
		public DateTime Date { get; set; }
		public Decimal Amount { get; set; }
	}
}
