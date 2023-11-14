namespace EcoCentre.Models.ViewModel
{
	public class CustomizationProvider : ICustomizationProvider
    {
        public string PageTitle { get; set; }
        public string CustomViewRoot { get; set; }
	    public bool ShowGraph { get; set; }
    }
}