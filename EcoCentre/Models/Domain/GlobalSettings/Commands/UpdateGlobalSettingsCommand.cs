using System.Linq;
using FluentValidation;

namespace EcoCentre.Models.Domain.GlobalSettings.Commands
{
    public class UpdateGlobalSettingsCommand
    {
        private readonly Repository<GlobalSettings> _globalSettingsRepository;
        private readonly GlobalSettingsViewModelValidator _validator;
        private readonly TaskRepository _taskRepository;

        public UpdateGlobalSettingsCommand(Repository<GlobalSettings> globalSettingsRepository, GlobalSettingsViewModelValidator validator, TaskRepository taskRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
            _validator = validator;
            _taskRepository = taskRepository;
        }

        public void Execute(GlobalSettingsViewModel model)
        {
            _validator.ValidateAndThrow(model);
            var globalSettings = _globalSettingsRepository.Query.FirstOrDefault();
            if (globalSettings == null)
            {
                globalSettings = new GlobalSettings();
            }
            globalSettings.MaxYearlyClientVisits = model.MaxYearlyClientVisits;
            globalSettings.MaxYearlyClientVisitsWarning = model.MaxYearlyClientVisitsWarning;
            globalSettings.AdminNotificationsEmail = model.AdminNotificationsEmail;
            globalSettings.ContainerFullNotificationsEmail = model.ContainerFullNotificationsEmail;
            globalSettings.QstTaxRate = model.QstTaxRate;
	        globalSettings.QstTaxLine = model.QstTaxLine;
			globalSettings.GstTaxRate = model.GstTaxRate;
            globalSettings.GstTaxLine = model.GstTaxLine;
            globalSettings.DefaultMaterialUnit = model.DefaultMaterialUnit;
            globalSettings.SessionTimeoutInMinutes = model.SessionTimeoutInMinutes;

            _globalSettingsRepository.Save(globalSettings);

            _taskRepository.RemoveTask(_taskRepository.FindTaskByName<BackgroundTaskData>("UpdateVisitsLimitExceededTask"));
        }
    }
}