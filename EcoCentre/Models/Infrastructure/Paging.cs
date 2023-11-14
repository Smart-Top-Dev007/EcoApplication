namespace EcoCentre.Models.Infrastructure
{
	public class Paging
	{
		public int TotalCount { get; set; }
		public int From { get; set; }
		public int PageSize { get; set; }

		// 1-based index of current page.
		public int CurrentPage
		{
			get
			{
				if (PageSize == 0) return 0;
				return From / PageSize + 1;
			}
		}
	}
}