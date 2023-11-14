using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Queries
{
    public class MaterialListQuery
    {
        private readonly Repository<Material> _materialRepository;
	    private readonly AuthenticationContext _context;

	    public MaterialListQuery(Repository<Material> materialRepository, AuthenticationContext context)
	    {
		    _materialRepository = materialRepository;
		    _context = context;
	    }

        public IEnumerable<Material> Execute(MaterialListQueryParam param)
        {
            var query = _materialRepository.Query;
	        if (!string.IsNullOrEmpty(param.Term))
	        {
		        query = query.Where(x => x.NameLower.Contains(param.Term.ToLower()));
				
			}
			if (param.Active || param.HasContainer)
			{
				query = query.Where(x => x.Active == true);
			}

			var result = query.OrderBy(x=>x.NameLower).ToList();


			// do this filtering in memory, as it does not work on mongo for some reason.
			//if (param.HasContainer)
			//{
			//	var hubId = _context.Hub.Id;
			//	result = result
			//		.Where(x => x.GetHubSettings(hubId, param.Municipality)?.HasContainer == true)
			//		.ToList();
			//}

			//if (param.OnlyCurrentHub == true)
			//{
			//	var hubId = _context.Hub.Id;
			//	result = result
			//		.Where(x => x.GetHubSettings(hubId, param.Municipality)?.IsActive == true)
			//		.ToList();
			//}

			return result;
        }
	}
}