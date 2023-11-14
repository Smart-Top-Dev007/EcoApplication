using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.OBNLReinvestments.Queries;
using FluentValidation;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class OBNLReinvestmentViewModelValidator : AbstractValidator<OBNLReinvestmentViewModel>
    {
        private readonly Repository<Domain.Clients.Client> _clientRepo;
        private readonly Domain.GlobalSettings.Queries.GlobalSettingsQuery _globalSettingsQuery;
        private readonly CompleteOBNLReinvestmentsListQuery _completeInvoiceListQuery;
        private readonly Repository<Material> _materialRepository;

        public OBNLReinvestmentViewModelValidator(Repository<Domain.Clients.Client> clientRepo, 
            Domain.GlobalSettings.Queries.GlobalSettingsQuery globalSettingsQuery, 
            CompleteOBNLReinvestmentsListQuery completeInvoiceListQuery, 
            Repository<Material> materialRepository)
        {
            _clientRepo = clientRepo;
            _globalSettingsQuery = globalSettingsQuery;
            _completeInvoiceListQuery = completeInvoiceListQuery;
            _materialRepository = materialRepository;

            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientId).Must(x => SelectedClient(x) != null).WithMessage("Choisissez le client");
            RuleFor(x => x.EmployeeName).NotEmpty().When(x =>
                {
                    var client = SelectedClient(x.ClientId);
                    if (client == null) return false;
                    return client.Category == "Municipality";
                });
            RuleFor(x => x.Materials)
                .Must(HaveAtLeastOneMaterial).WithMessage("Choisissez au moins un matériau");
                
            RuleFor(x => x.Materials).SetCollectionValidator(new OBNLReinvestmentMaterialViewModelValidator());

            RuleFor(x => x.Materials)
                .Must(HaveUniqueMaterials)
                .WithMessage("Matériaux ne peuvent pas être pris plusieurs foisv")
                .When(x => x.Materials != null && x.Materials.Count > 0);

            RuleFor(x => x.ClientId)
                .Must(x =>
                {
                    var client = SelectedClient(x);
                    var numVisits = _completeInvoiceListQuery.Execute(new CompleteOBNLReinvestmentsListQueryParams
                    {
                        ClientId = client.Id
                    }).Count(i => !i.IsExcluded);

                    if (null != client.PersonalVisitsLimit && client.PersonalVisitsLimit > 0)
                    {
                        return numVisits < client.PersonalVisitsLimit;
                    }
                    else
                    {
                        var globalVisitsLimit = _globalSettingsQuery.Execute().MaxYearlyClientVisits;
                        if (globalVisitsLimit > 0)
                        {
                            return numVisits < globalVisitsLimit;
                        }                       
                    }

                    return true;
                })
                .Unless(i =>
                {
                    var materialIds = i.Materials.Select(x => x.Id);
                    var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id));

                    bool isInvoiceExcluded = true;
                    foreach (var material in i.Materials)
                    {
                        isInvoiceExcluded &= materials.First(x => x.Id == material.Id).IsExcluded;
                    }
                    return isInvoiceExcluded;
                })
                .WithMessage("Le client a dépassé le nombre annuel maximal de visites autorisé");
        }


        private bool HaveAtLeastOneMaterial(IList<OBNLReinvestmentMaterialViewModel> arg)
        {
            return arg != null && arg.Count > 0;
        }
        private bool HaveUniqueMaterials(IList<OBNLReinvestmentMaterialViewModel> arg)
        {
            return arg.Count == arg.Select(x => x.Id).Distinct().Count();
        }

        private readonly IDictionary<string, Clients.Client> _clientsCache = new Dictionary<string, Clients.Client>();
        private Clients.Client SelectedClient(string id)
        {
            
            if (!_clientsCache.ContainsKey(id))
            {
                _clientsCache.Add(id, _clientRepo.FindOne(id));
            }
            return _clientsCache[id];
        }

        // add collection operation here
    }
}