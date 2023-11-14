using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.MoneticoPayments;
using FluentValidation;
using NLog;

namespace EcoCentre.Models.Domain.Payments
{
	public class SavePaymentSettingsCommand
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly AuthenticationContext _context;
		private readonly Repository<MoneticoConfiguration> _repository;
		private readonly SavePaymentSettingsCommandValidator _validator;

		public SavePaymentSettingsCommand(
			AuthenticationContext context,
			Repository<MoneticoConfiguration> repository
			, SavePaymentSettingsCommandValidator validator)
		{
			_context = context;
			_repository = repository;
			_validator = validator;
		}

		public string Tpe { get; set; }
		public string Currency { get; set; }
		public string Language { get; set; }
		public string Company { get; set; }
		public string Key { get; set; }
		public MoneticoEnvironment Environment { get; set; }

		public void Execute()
		{
			_validator.ValidateAndThrow(this);

			var config = _repository.Query.FirstOrDefault();
			if (config == null)
			{
				config = new MoneticoConfiguration();
			}

			var fields = new List<string>();
			if (Tpe != config.Tpe)
			{
				fields.Add(nameof(config.Tpe) + $"( new value: {Tpe})");
				config.Tpe = Tpe.ToUpper();
			}

			if (Company != config.Company)
			{
				fields.Add(nameof(config.Company) + $"( new value: {Company})");
				config.Company = Company;
			}

			if (Currency != config.Currency)
			{
				fields.Add(nameof(config.Currency) + $"( new value: {Currency})");
				config.Currency = Currency;
			}

			if (Key != config.Key)
			{
				fields.Add(nameof(config.Key));
				config.Key = Key;
			}

			if (Language != config.Language)
			{
				fields.Add(nameof(config.Language) + $"( new value: {Language})");
				config.Language = Language;
			}

			if (Environment != config.Environment)
			{
				fields.Add(nameof(config.Environment) + $"( new value: {Environment})");
				config.Environment = Environment;
			}
			
			Logger.Info(
				$"Updating payment information. Changed by: {_context.User.Name} ({_context.User.Login}). Changed fields: {string.Join(", ", fields)}.");

			_repository.Save(config);
		}
	}
}