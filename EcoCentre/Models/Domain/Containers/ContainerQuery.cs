using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.ViewModel.Container;

namespace EcoCentre.Models.Domain.Containers
{
	public class ContainerQuery
	{
		private readonly Repository<Container> _containerRepository;
		private readonly Repository<Material> _materialRepository;
		private readonly Repository<Hub> _hubRepository;
		private readonly AuthenticationContext _context;

		public ContainerQuery(Repository<Container> containerRepository,
			Repository<Material> materialRepository,
			Repository<Hub> hubRepository,
			AuthenticationContext context
			)
		{
			_containerRepository = containerRepository;
			_materialRepository = materialRepository;
			_hubRepository = hubRepository;
			_context = context;
		}

		public string Id { get; set; }
		public string MaterialId { get; set; }
		public bool OnlyCurrentHub { get; set; }
		public bool? Deleted { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }

		public PagedList<ContainerViewModel> Execute()
		{
			if (PageSize > 1000 || PageSize < 1)
			{
				PageSize = 20;
			}
			if (Page < 1)
			{
				Page = 1;
			}

			var query = _containerRepository.Query;

			if (Deleted == null)
			{
				query = query.Where(x => x.IsDeleted == false);
			}
			else
			{
				query = query.Where(x => x.IsDeleted == Deleted);
			}

			if (!string.IsNullOrWhiteSpace(Id))
			{
				query = query.Where(x => x.Id == Id);
			}

			if (!string.IsNullOrWhiteSpace(MaterialId))
			{
				query = query.Where(x => x.Materials.Any(m => m.Id == MaterialId));
			}

			if (OnlyCurrentHub)
			{
				var hubId = _context.Hub.Id;
				query = query.Where(x => x.HubId == hubId);
			}

			var containers = query
				.OrderByDescending(x => x.DateAdded)
				.Skip((Page - 1) * PageSize)
				.Take(PageSize)
				.ToList();

			var count = query.Count();

			if (!containers.Any())
			{
				return new PagedList<ContainerViewModel>(null, Page, PageSize, count);
			}

			var materialIds = containers.SelectMany(x => x.Materials.Select(m => m.Id)).Distinct().ToList();
			var materials = _materialRepository.Query.Where(x => materialIds.Contains(x.Id)).ToDictionary(x => x.Id);

			var hubIds = containers.Select(x => x.HubId).Distinct().ToList();
			var hubs = _hubRepository.Query.Where(x => hubIds.Contains(x.Id)).ToDictionary(x => x.Id);

			var result = containers.Select(x => new ContainerViewModel
			{
				Id = x.Id,
				AlertAtAmount = x.AlertAtAmount,
				Capacity = x.Capacity,
				DateAdded = x.DateAdded,
				DateOfLastAlert = x.DateOfLastAlert,
				FillAmount = x.FillAmount,
				Materials = x.Materials.Select(m => new MaterialViewModel
				{
					Id = m.Id,
					Name = materials.ContainsKey(m.Id) ? materials[m.Id].Name : " "
				}
				).ToList(),
				Number = x.Number,
				HubId = x.HubId,
				IsDeleted = x.IsDeleted,
				DateDeleted = x.DateDeleted,
				HubName = hubs[x.HubId].Name
			})
				.ToList();
		

			return new PagedList<ContainerViewModel>(result, Page, PageSize, count);
		}

	}
}