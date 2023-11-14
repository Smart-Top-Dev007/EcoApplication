using System.Web;
using EcoCentre.Controllers;
using EcoCentre.Models.Infrastructure;
using FluentValidation;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class UpdateGiveawayCommand
	{
		private readonly UpdateGiveawayCommandValidator _validator;
		private readonly Repository<GiveawayItem> _repository;

		public UpdateGiveawayCommand(UpdateGiveawayCommandValidator validator, Repository<GiveawayItem> repository)
		{
			_validator = validator;
			_repository = repository;
		}

		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public decimal Price { get; set; }
		public string ImageId { get; set; }
		
		public void Execute()
		{
			_validator.ValidateAndThrow(this);

			var item = _repository.FindOne(Id);

			if (item == null || item.IsDeleted)
			{
				throw new NotFoundException();
			}

			item.Description = Description;
			item.DescriptionLower = Description?.ToLower();
			item.Title = Title;
			item.TitleLower = Title?.ToLower();
			item.Price = Price;
			item.Type = Type;
			item.TypeLower = Type?.ToLower();
			item.ImageId = ImageId;

			_repository.Save(item);
		}

		
	}
}