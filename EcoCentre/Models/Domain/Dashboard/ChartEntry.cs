namespace EcoCentre.Models.Domain.Dashboard
{
    public class ChartDataSet
    {
		public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public int[] data { get; set; }
    }

	public class ChartData
	{
		public string[] labels { get; set; }
		public ChartDataSet[] datasets { get; set; }
	}
}