using System.Linq;

namespace EcoCentre.Models.Domain.GlobalSettings.Queries
{
    public class GlobalSettingsQuery
    {
        private readonly Repository<GlobalSettings> _globalSettingsRepository;

        public GlobalSettingsQuery(Repository<GlobalSettings> globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
        }

        public GlobalSettings Execute()
        {
            var globalSettings = _globalSettingsRepository.Query.FirstOrDefault();
            if (globalSettings == null)
            {
                globalSettings = new GlobalSettings();
            }
            return globalSettings;
        }
    }
}