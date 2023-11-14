using System.Linq;
using EcoCentre.Models.Domain.Hubs;
using FluentValidation;

namespace EcoCentre.Models.Domain.Clients.Commands
{
	public class LoginDataValidator : AbstractValidator<LoginData>
	{
		private readonly Repository<Hub> _repository;

		public LoginDataValidator(Repository<Hub> repository)
		{
			_repository = repository;
			RuleFor(x => x.Login).NotEmpty().WithName(Resources.Model.User.Username);
			RuleFor(x => x.Password).NotEmpty().WithName(Resources.Model.User.Password);
			//RuleFor(x => x.HubId)
			//	.NotEmpty()
			//	.When(HubsExist)
			//	.WithMessage("Choisissez eco centre");
		}

		private bool HubsExist(LoginData arg)
		{
			return _repository.Query.Any();
		}
	}
}