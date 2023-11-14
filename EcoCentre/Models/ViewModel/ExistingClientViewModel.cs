namespace EcoCentre.Models.ViewModel
{
	public class ExistingClientViewModel : ClientViewModel
	{ 
        public bool UpdateOnlyStatus { get; set; }
        public bool UpdateOnlyPersonalVisitsLimit { get; set; }
	}
}