using System;
using System.Web;
using EcoCentre.Controllers;
using EcoCentre.Models.Domain.User;
using FluentValidation;

namespace EcoCentre.Models.Domain.Giveaway
{
	public class CreateGiveawayCommand
	{
		private readonly CreateGiveawayCommandValidator _validator;
		private readonly Repository<GiveawayItem> _repository;
		private readonly AuthenticationContext _context;
		private readonly Sequences _sequences;

		public CreateGiveawayCommand(
			CreateGiveawayCommandValidator validator,
			Repository<GiveawayItem> repository,
			AuthenticationContext context,
			Sequences sequences)
		{
			_validator = validator;
			_repository = repository;
			_context = context;
			_sequences = sequences;
		}
		public string Title { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public decimal Price { get; set; }
		public string ImageId { get; set; }


		public void Execute()
		{
			_validator.ValidateAndThrow(this);

			var hub = _context.Hub;
			
			var item = new GiveawayItem
			{
				DateAdded = DateTime.UtcNow,
				Description = Description,
				DescriptionLower = Description?.ToLower(),
				Title = Title,
				TitleLower = Title?.ToLower(),
				Price = Price,
				Type = Type,
				TypeLower = Type?.ToLower(),
				ImageId = ImageId,
				HubId = hub.Id,
				HubName = hub.Name,
				CreatedByUserId = _context.User.Id,
				IsPublished = true,
				IsGivenAway = false,
				IsDeleted = false,
				SequenceNo = _sequences.GetNext("Giveaway")
			};

			_repository.Insert(item);
		}
	}
}