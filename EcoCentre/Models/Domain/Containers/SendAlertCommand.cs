using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Containers
{
	public class SendAlertCommand
	{
		private readonly Repository<Container> _repository;
		private readonly Repository<Hub> _hubRepository;
		private readonly Repository<GlobalSettings.GlobalSettings> _settingsRepository;
		private readonly Repository<User.User> _userRepository;
		private readonly Repository<Material> _materialRepository;
		private readonly Mailer _mailer;

		public SendAlertCommand(
			Repository<Container> repository,
			Repository<Hub> hubRepository,
			Repository<GlobalSettings.GlobalSettings> settingsRepository,
			Repository<User.User> userRepository,
			Repository<Material> materialRepository,
			Mailer mailer)
		{
			_repository = repository;
			_hubRepository = hubRepository;
			_settingsRepository = settingsRepository;
			_userRepository = userRepository;
			_materialRepository = materialRepository;
			_mailer = mailer;
		}

		public AlertResult Execute(string containerId, string userId = null)
		{
			var container = _repository.FindOne(containerId);
			if (container == null || container.IsDeleted)
			{
				throw new NotFoundException(typeof(Container), containerId);
			}

			var hub = _hubRepository.FindOne(container.HubId);
			if (hub == null)
			{
				throw new NotFoundException(typeof(Hub), container.HubId);
			}

			var materialIds = container.Materials.Select(x => x.Id).ToList();
			var materials = _materialRepository.Query.Where(x=> materialIds.Contains(x.Id)).ToList();
			if (!materials.Any())
			{
				throw new NotFoundException(typeof(Material), $"Could not find any material with ids: {materialIds.JoinBy(", ")}");
			}

			string employeeName = null;
			if (userId != null)
			{
				var user = _userRepository.FindOne(userId);
				employeeName = user?.Name ?? user?.Login;
			}

			var model = new ContainerFullAlertModel
			{
				HubName = hub.Name,
				ContainerNumber = container.Number,
				EmployeeName = employeeName,
				Materials = materials
			};

			var address = _settingsRepository.Query.FirstOrDefault()?.ContainerFullNotificationsEmail;
			if (string.IsNullOrWhiteSpace(address))
			{
				throw new Exception("Container alert email destination address is not specified.");
			}

			SendMail(model, address);

			container.DateOfLastAlert = DateTime.UtcNow;
			_repository.Save(container);

			return new AlertResult
			{
				DateOfLastAlert = container.DateOfLastAlert
			};
		}
	
		public void SendMail(ContainerFullAlertModel model, string mailAddress)
		{

			var materials = model.Materials.Select(x => x.Name).Distinct().JoinBy(", ");
			var content = $"<p>Remplacer le conteneur \"{model.ContainerNumber}\" en {materials} de l'écocentre de {model.HubName}.</p>";
			if (!string.IsNullOrWhiteSpace(model.EmployeeName))
			{
				content += $"<p>Employé sur place: {model.EmployeeName}</p>";
			}

			var mailSubject = "Remplacer le conteneur";
			_mailer.Send(mailAddress, mailSubject, content);
		}
	}

	public class ContainerFullAlertModel
	{
		public string ContainerNumber { get; set; }
		public string HubName { get; set; }
		public string EmployeeName { get; set; }
		public List<Material> Materials { get; set; }
	}
}