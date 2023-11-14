using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using EcoCentre.Models.MoneticoPayments;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IntegrationTests
{
	[TestClass]
	public class MoneticoTests
	{
		[TestMethod]
		public void PaymentProcessor_Should_Calculate_Mac()
		{

			var paymentRequest = new PaymentFields
			{
				Version = "3.0",
				Tpe = "0000001",
				Date = "05%2F12%2F2006%3A11%3A55%3A23",
				Amount = "1.01EUR",
				Reference = "ref164014",
				FreeText = "FreeTextExample",

			};
			var mac = PaymentProcessor.CalculateMac(paymentRequest, new byte[] { 0x01, 0x02 });

			mac.Should().Be("39644DCDBB5C1AADCE8816AAED1FBEEE76618403");
		}

		[TestMethod]
		public void PaymentResponseValidator_should_indicate_invalid_signature()
		{
			var response = @"TPE=1234567&date=05%2f12%2f2006%5fa%5f11%3a55%3a23&montant=62%2e75CA" +
			               "D&reference=ABERTYP00145&MAC=e4359a2c18d86cf2e4b0e646016c202e89947b0" +
			               "4&texte-libre=LeTexteLibre&code-retour=paiement&cvx=oui&vld=1208&brand=VI&status3ds=1&numauto=010101" +
			               "&originecb=CAN&bincb=010101&hpancb=74E94B03C22D786E0F2C2CADBFC1C00B0" +
			               "04B7C45&ipclient=127%2e0%2e0%2e1&originetr=CAN&veres=Y&pares=Y";

			var validator = new PaymentResponseValidator();
			var result = validator.ValidateResponse(response, "001122");


			result.IsValid.Should().Be(false);
		}

		[TestMethod]
		public void PaymentResponseValidator_should_validate_valid_signature()
		{
			var response = @"TPE=1234567&date=05%2f12%2f2006%5fa%5f11%3a55%3a23&montant=62%2e75CA" +
						   "D&reference=ABERTYP00145&MAC=03669D25DFE6B61669CCD32FBDFBEE4473D2F56D" +
			               "&texte-libre=LeTexteLibre&code-retour=paiement&cvx=oui&vld=1208&brand=VI&status3ds=1&numauto=010101" +
			               "&originecb=CAN&bincb=010101&hpancb=74E94B03C22D786E0F2C2CADBFC1C00B0" +
			               "04B7C45&ipclient=127%2e0%2e0%2e1&originetr=CAN&veres=Y&pares=Y";

			var validator = new PaymentResponseValidator();
			var result = validator.ValidateResponse(response, "001122");


			result.IsValid.Should().Be(true);
		}

		[TestMethod]
		public void PaymentResponseValidator_should_return_valid_amount_independent_of_culture()
		{

			var culture = CultureInfo.CreateSpecificCulture("fr-CA");
			CultureInfo.CurrentCulture = culture;

			var response = @"TPE=1234567&date=05%2f12%2f2006%5fa%5f11%3a55%3a23&montant=62%2e75CA" +
						   "D&reference=ABERTYP00145&MAC=03669D25DFE6B61669CCD32FBDFBEE4473D2F56D" +
			               "&texte-libre=LeTexteLibre&code-retour=paiement&cvx=oui&vld=1208&brand=VI&status3ds=1&numauto=010101" +
			               "&originecb=CAN&bincb=010101&hpancb=74E94B03C22D786E0F2C2CADBFC1C00B0" +
			               "04B7C45&ipclient=127%2e0%2e0%2e1&originetr=CAN&veres=Y&pares=Y";

			var validator = new PaymentResponseValidator();
			var result = validator.ValidateResponse(response, "001122");


			result.IsValid.Should().Be(true);
			result.Amount.Should().Be(62.75m);
		}
	}
}
