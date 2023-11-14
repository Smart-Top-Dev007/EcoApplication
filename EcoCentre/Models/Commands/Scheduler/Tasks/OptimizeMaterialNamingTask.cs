using System;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Commands.Scheduler.Tasks
{
    public class OptimizeMaterialNamingTask :Task<OptimizeMaterialNamingTaskData>
    {
        private readonly Repository<Material> _materialRepository;

        public OptimizeMaterialNamingTask(TaskRepository repository, Repository<Material> materialRepository, BgTaskInterval interval ) : base(repository, interval)
        {
            _materialRepository = materialRepository;
        }

        protected override void DoWork(DateTime execTime)
        {
            var materials = _materialRepository.Query.Where(x => x.UpdatedAt >= Data.LastRun && x.UpdatedAt < execTime).ToList();
            foreach (var material in materials)
            {
                if (material.Name == null) material.Name = "";
                material.NameLower = material.Name.ToLower();
                _materialRepository.Save(material);
            }
        }
    }
}