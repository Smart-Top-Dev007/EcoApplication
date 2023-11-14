namespace EcoCentre.Models.ViewModel
{
	public interface ICustomizationProvider
	{
		string PageTitle { get; }
		string CustomViewRoot { get; }
        bool ShowGraph { get; set; }

	}
}