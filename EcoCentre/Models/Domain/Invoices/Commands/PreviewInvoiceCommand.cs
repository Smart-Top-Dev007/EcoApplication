using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.Materials;
using FluentValidation;

namespace EcoCentre.Models.Domain.Invoices.Commands
{
	public class PreviewInvoiceCommand
	{
		
		private readonly InvoiceViewModelValidator _invoiceViewModelValidator;
		private readonly Repository<Client> _clientRepository;
		private readonly Repository<Material> _materialRepository;
		private readonly InvoiceMapper _invoiceMapper;

		public PreviewInvoiceCommand(
			InvoiceViewModelValidator invoiceViewModelValidator,
			Repository<Client> clientRepository,
			Repository<Material> materialRepository,
			InvoiceMapper invoiceMapper)
		{ 			
			_invoiceViewModelValidator = invoiceViewModelValidator;
			_clientRepository = clientRepository;
			_materialRepository = materialRepository;
			_invoiceMapper = invoiceMapper;
		}

		public InvoiceDetails Execute(InvoiceViewModel vm)
		{

			_invoiceViewModelValidator.ValidateAndThrow(vm);
			var sequentialNo = 0;
			
			var invoice = _invoiceMapper.CreateInvoice(vm, sequentialNo, false);

			var client = _clientRepository.FindOne(vm.ClientId);


			var materials = GetMaterials(vm);


			var result = new InvoiceDetails(invoice, client);

			var isExcluded = true;
			foreach (var material in invoice.Materials)
			{
				var materialDetails = new InvoiceDetailsMaterial();
				var originalMaterial = materials[material.MaterialId];
				materialDetails.Name = originalMaterial.Name;
				materialDetails.Quantity = material.Quantity;
				materialDetails.Weight = material.Weight;
				materialDetails.Unit = originalMaterial.Unit;
				materialDetails.Price = material.Price;
				materialDetails.IsUsingFreeAmount = material.IsUsingFreeAmount;
				materialDetails.Amount = material.Amount;
				result.Materials.Add(materialDetails);
				isExcluded &= originalMaterial.IsExcluded;
			}

			result.IsExcluded = isExcluded;
			result.IsOBNL = client.Category == "OBNL";

			return result;
		}

		private Dictionary<string, Material> GetMaterials(InvoiceViewModel vm)
		{
			if (vm.Materials == null)
			{
				return new Dictionary<string, Material>();
			}
			var materialIds = vm.Materials
				.Select(m => m.Id)
				.Distinct();

			var materials = _materialRepository.Query
				.Where(x => materialIds.Contains(x.Id))
				.ToDictionary(x => x.Id);
			return materials;
		}
	}
}