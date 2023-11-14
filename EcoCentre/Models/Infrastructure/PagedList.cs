using System.Collections.Generic;

namespace EcoCentre.Models.Infrastructure
{
	public class PagedList<T>
	{

		public PagedList(IEnumerable<T> list, Paging paging)
		{
			Items = list;
			Paging = paging;
		}
		public PagedList(IEnumerable<T> list, int page, int pageSize, int totalCount)
		{
			Items = list;
			Paging = new Paging
			{
				From = (page - 1) * pageSize,
				PageSize = pageSize,
				TotalCount = totalCount
			};
		}

		public IEnumerable<T> Items { get; }
		public Paging Paging { get; }
	}
}