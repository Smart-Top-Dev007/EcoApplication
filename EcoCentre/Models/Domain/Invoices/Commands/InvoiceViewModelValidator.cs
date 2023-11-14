using System;
using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using FluentValidation;
using FluentValidation.Validators;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
	public class InvoiceViewModelValidator : AbstractValidator<InvoiceViewModel>
    {
        private readonly Repository<Client> _clientRepo;
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly CompleteInvoicesListQuery _completeInvoiceListQuery;
	    private readonly Repository<Invoice> _invoiceRepository;
	    private readonly AuthenticationContext _context;
	    private readonly IDictionary<string, Client> _clientsCache = new Dictionary<string, Client>();
	    private readonly Dictionary<string, Material> _materials;

		public InvoiceViewModelValidator(
			Repository<Client> clientRepo,
			GlobalSettingsQuery globalSettingsQuery,
			CompleteInvoicesListQuery completeInvoiceListQuery,
			Repository<Material> materialRepository,
			Repository<Invoice> invoiceRepository,
			AuthenticationContext context)
        {
            _clientRepo = clientRepo;
            _globalSettingsQuery = globalSettingsQuery;
            _completeInvoiceListQuery = completeInvoiceListQuery;
	        _invoiceRepository = invoiceRepository;
	        _context = context;
	        _materials = materialRepository.Query.ToDictionary(x => x.Id);

			RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientId).Must(x => SelectedClient(x) != null).WithMessage("Choisissez le client");
            RuleFor(x => x.EmployeeName).NotEmpty().When(x =>
                {
                    var client = SelectedClient(x.ClientId);
                    if (client == null) return false;
                    return client.Category == "Municipality";
                });
            RuleFor(x => x.Materials)
                .Must(HaveAtLeastOneMaterial)
				.When(NoGiveawaysAdded)
				.WithMessage("Choisissez au moins un matériau");
                
            RuleFor(x => x.Materials).SetCollectionValidator(new InvoiceMaterialViewModelValidator());

            RuleFor(x => x.Materials)
                .Must(HaveUniqueMaterials)
                .WithMessage("Matériaux ne peuvent pas être pris plusieurs fois")
                .When(x => x.Materials != null && x.Materials.Count > 0);
			
	        RuleForEach(x => x.Materials)
				.Must(NotExceedHubAmountLimit)
				.WithMessage(GetExceededHubAmountLimitErrorMessage)
                .When(x => x.Materials != null && x.Materials.Count > 0)
		        .When(ClientIdIsProvided);

	   //     RuleForEach(x => x.Materials)
				//.Must(BeActiveInHub)
				//.WithMessage("Le matériel n'est pas actif")
    //            .When(x => x.Materials != null && x.Materials.Count > 0)
		  //      .When(ClientIdIsProvided);

			RuleForEach(x => x.Materials)
				.Must(NotExceedHubVisitLimit)
				.WithMessage(GetExceededHubVisitLimitErrorMessage)
                .When(x => x.Materials != null && x.Materials.Count > 0)
		        .When(ClientIdIsProvided);

			RuleForEach(x => x.Materials)
				.Must(HaveProofOfResidence)
				.WithMessage(GetProofOfResidenceErrorMessage)
                .When(x => x.Materials != null && x.Materials.Count > 0)
		        .When(ClientIdIsProvided);

			RuleFor(x => x.ClientId)
                .Must(NotExceedVisitsLimit)
                .Unless(InvoiceIsExcluded)
                .WithMessage("Le client a dépassé le nombre annuel maximal de visites autorisé")
	            .When(ClientIdIsProvided);
		}

	    private bool BeActiveInHub(InvoiceViewModel model, InvoiceMaterialViewModel materialViewModel, PropertyValidatorContext validatorContext)
		{
			var hub = _context.Hub;
		    if (hub == null)
		    {
			    return true;
		    }
		    var material = _materials[materialViewModel.Id];
		    if (material == null)
		    {
			    return true;
		    }

		    var client = SelectedClient(model.ClientId);
		    var settings = material.GetHubSettings(hub.Id, client.Address.CityId);
		    if (settings?.IsActive == true)
		    {
			    return true;
		    }
		    return false;
		}

	    private string GetProofOfResidenceErrorMessage(InvoiceViewModel root, InvoiceMaterialViewModel model)
	    {
			var material = _materials[model.Id];
		    return $@"Le client doit fournir une preuve d'identité pour ""{ material.Name}"".Cochez la case par la suite.";
		}

	    private bool HaveProofOfResidence(InvoiceViewModel model, InvoiceMaterialViewModel materialViewModel, PropertyValidatorContext validatorContext)
	    {
		    if (materialViewModel.ProvidedProofOfResidence)
		    {
			    return true;
		    }
			var hub = _context.Hub;
		    if (hub == null)
		    {
			    return true;
		    }
		    var material = _materials[materialViewModel.Id];
		    if (material == null)
		    {
			    return true;
		    }

		    var client = SelectedClient(model.ClientId);
			var settings = material.GetHubSettings(hub.Id, client.Address.CityId);
		    if (settings?.RequireProofOfResidence != true)
		    {
			    return true;
		    }
		    return false;
	    }

	    private string GetExceededHubVisitLimitErrorMessage(InvoiceViewModel root, InvoiceMaterialViewModel model)
	    {
		    var hub = _context.Hub;
		    var material = _materials[model.Id];
		    var client = SelectedClient(root.ClientId);
			var hubSettings = material.GetHubSettings(hub.Id, client.Address.CityId);
			return
			    $@"Limite de visite du regroupment dépassée pour cet materiel ""{ material.Name}"".La limite pour { hub.Name} est { hubSettings.MaxVisits} visites par an.";
		}
		private string GetExceededHubAmountLimitErrorMessage(InvoiceViewModel root, InvoiceMaterialViewModel model)
	    {
		    var hub = _context.Hub;
		    var material = _materials[model.Id];
		    var client = SelectedClient(root.ClientId);
		    var hubSettings = material.GetHubSettings(hub.Id, client.Address.CityId);
			return
			    $@"Quantité maximale de matériau ""{ material.Name}"" dans { hub.Name} est { hubSettings.MaxAmountPerVisit} { material.Unit}.";

		}

	    private bool NotExceedHubAmountLimit(InvoiceViewModel root, InvoiceMaterialViewModel model, PropertyValidatorContext context)
	    {
		    var hub = _context.Hub;
		    if (hub == null)
		    {
			    return true;
		    }
			var material = _materials[model.Id];
		    if (material == null)
		    {
			    return true;
		    }

			var client = SelectedClient(root.ClientId);
		    var hubSettings = material.GetHubSettings(hub.Id, client.Address.CityId);
			if (hubSettings?.MaxAmountPerVisit == null || hubSettings.MaxAmountPerVisit == 0)
		    {
			    return true;
		    }
			return hubSettings.MaxAmountPerVisit >= model.Quantity;
		}
		
	    private bool NotExceedHubVisitLimit(InvoiceViewModel root, InvoiceMaterialViewModel model, PropertyValidatorContext context)
		{
			var hub = _context.Hub;
			if (hub == null)
			{
				return true;
			}
			var material = _materials[model.Id];
			if (material == null)
			{
				return true;
			}

			var client = SelectedClient(root.ClientId);
			var hubSettings = material.GetHubSettings(hub.Id, client.Address.CityId);
			if (hubSettings?.MaxVisits == null || hubSettings.MaxVisits == 0)
			{
				return true;
			}

			var thisYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

			var visitCount = _invoiceRepository.Query
				.Count(x =>
					x.ClientId == root.ClientId
					&& x.Materials.Any(m => m.MaterialId == model.Id)
					&& x.CreatedAt > thisYear
					&& x.Center.Id == hub.Id);

			var exceeds = hubSettings.MaxVisits <= visitCount;
			return !exceeds;
		}

	    private bool NoGiveawaysAdded(InvoiceViewModel arg)
	    {
		    return arg.GiveawayItems?.Any() != true;
	    }

	    private static bool ClientIdIsProvided(InvoiceViewModel invoice)
	    {
		    return !string.IsNullOrWhiteSpace(invoice.ClientId) && invoice.ClientId != "0";
	    }

	    private bool InvoiceIsExcluded(InvoiceViewModel invoice)
	    {
		    if (invoice.Materials?.Any()!= true)
		    {
			    return true;
		    }
			var materialIds = invoice.Materials.Select(x => x.Id);
		    var materials = _materials.Values.Where(x => materialIds.Contains(x.Id)).ToList();

		    var isInvoiceExcluded = true;
		    foreach (var material in invoice.Materials)
		    {
			    isInvoiceExcluded &= materials.First(x => x.Id == material.Id).IsExcluded;
		    }
		    return isInvoiceExcluded;
	    }

	    private bool NotExceedVisitsLimit(string clientId)
	    {
		    var client = SelectedClient(clientId);
		    var numVisits = _completeInvoiceListQuery.Execute(new CompleteInvoicesListQueryParams
		    {
			    ClientId = client.Id
		    }).Count(i => !i.IsExcluded);

		    if (null != client.PersonalVisitsLimit && client.PersonalVisitsLimit > 0)
		    {
			    return numVisits < client.PersonalVisitsLimit;
		    }
		    var globalVisitsLimit = _globalSettingsQuery.Execute().MaxYearlyClientVisits;
		    if (globalVisitsLimit > 0)
		    {
			    return numVisits < globalVisitsLimit;
		    }

		    return true;
	    }


	    private bool HaveAtLeastOneMaterial(IList<InvoiceMaterialViewModel> arg)
        {
            return arg != null && arg.Count > 0;
        }
        private bool HaveUniqueMaterials(IList<InvoiceMaterialViewModel> arg)
        {
            return arg.Count == arg.Select(x => x.Id).Distinct().Count();
        }

        
	    private Client SelectedClient(string id)
        {
            if (!_clientsCache.ContainsKey(id))
            {
                _clientsCache.Add(id, _clientRepo.FindOne(id));
            }
            return _clientsCache[id];
        }
		
    }
}