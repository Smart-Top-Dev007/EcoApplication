using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Limits;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Materials.Commands
{
    public class MergeMaterialsCommand
    {
        private readonly Repository<Material> _materialRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        private readonly Repository<MaterialBrought> _materialBroughtRepository;
        private readonly Repository<CachedMaterialsByAddressQuery> _cachedMaterialsByAddressBundle;
        private readonly Repository<MaterialByAddress> _cachedMaterialsByAddress;

        private readonly Repository<LimitStatus> _limitsRepository;

        public MergeMaterialsCommand(Repository<Material> materialRepository, Repository<Invoice> invoiceRepository,
            Repository<MaterialBrought> materialBroughtRepository, Repository<CachedMaterialsByAddressQuery> cachedMaterialsByAddressBundle,
            Repository<MaterialByAddress> cachedMaterialsByAddress, Repository<LimitStatus> limitsRepository)
        {
            _materialRepository = materialRepository;
            _invoiceRepository = invoiceRepository;

            _materialBroughtRepository = materialBroughtRepository;
            _cachedMaterialsByAddressBundle = cachedMaterialsByAddressBundle;
            _cachedMaterialsByAddress = cachedMaterialsByAddress;

            _limitsRepository = limitsRepository;
        }

        private void CleanLimits(Material mergeSource)
        {
            var srcLimits = _limitsRepository.Query.Where(x => x.Limits.Any(l => l.Materials.Any(m => m.MaterialId == mergeSource.Id)));
            foreach (var srcLimit in srcLimits)
            {
                foreach (var srcYearLimit in srcLimit.Limits)
                {
                    // removing material from limits
                    var srcMatLimit = srcYearLimit.Materials.FirstOrDefault(x => x.MaterialId == mergeSource.Id);
                    srcYearLimit.Materials.Remove(srcMatLimit);
                }
            }
        }
        private void UpdateLimits(Invoice invoice)
        {
            var mateiralIds = invoice.Materials.Select(x => x.MaterialId).ToList();
            var materials = _materialRepository.Query.Where(x => mateiralIds.Contains(x.Id)).ToList();
            var limits = _limitsRepository.Query.FirstOrDefault(x => x.Address.Id == invoice.Address.Id);
            if (limits == null)
            {
                limits = new LimitStatus
                {
                    Address = invoice.Address,
                    Limits = new List<LimitStatusYear>()
                };
            }
            limits.UpdateLimits(invoice, materials);
            _limitsRepository.Save(limits);
        }

        public Material Execute(MergeCommandParams @params)
        {
            // all cached reports must be dropped
            _cachedMaterialsByAddressBundle.DropCollection();
            _cachedMaterialsByAddress.DropCollection();

            var mergeDestId = @params.MergeDest;
            var mergeDestMaterial = _materialRepository.FindOne(mergeDestId);

            var mergeSources = @params.MergeSourcesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var mergeSourceId in mergeSources)
            {
                var mergeSourceMaterial = _materialRepository.FindOne(mergeSourceId);

                var invoicesToModifyList = _invoiceRepository.Query.Where(x => x.Materials.Any(m => m.MaterialId == mergeSourceId)).ToList(); // it is way faster to work in memory
                foreach (var invoice in invoicesToModifyList)
                {
                    foreach (var material in invoice.Materials)
                    {
                        if (material.MaterialId == mergeSourceId)
                        {
                            material.MaterialId = mergeDestId;
                        }
                    }
                    _invoiceRepository.Save(invoice);
                    UpdateLimits(invoice);
                }
                var materialsBroughtToModifyList = _materialBroughtRepository.Query.Where(x => x.MaterialId == mergeSourceId).ToList();
                foreach (var matBrought in materialsBroughtToModifyList)
                {
                    matBrought.MaterialId = mergeDestId;
                    matBrought.MaterialName = mergeDestMaterial.Name;
                    matBrought.MaterialNameLower = mergeDestMaterial.NameLower;
                    matBrought.Unit = mergeDestMaterial.Unit;

                    _materialBroughtRepository.Save(matBrought);
                }


                CleanLimits(mergeSourceMaterial);

                _materialRepository.Remove(mergeSourceMaterial);

            }

            _materialBroughtRepository.ReIndex();
            _limitsRepository.ReIndex();

            return mergeDestMaterial;
        }
    }
}