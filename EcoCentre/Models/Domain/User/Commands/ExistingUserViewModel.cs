namespace EcoCentre.Models.Domain.User.Commands
{
    public class ExistingUserViewModel : UserViewModel
    {
        public ExistingUserViewModel()
        {
            
        }

        public ExistingUserViewModel(User user)
        {
            Id = user.Id;
            IsAdmin = user.IsAdmin || user.IsGlobalAdmin;
            IsGlobalAdmin = user.IsGlobalAdmin;
            IsReadOnly = user.IsReadOnly;
            Email = user.Email;
            Login = user.Login;
            IsAgent = !user.IsAdmin && !user.IsGlobalAdmin;
	        IsLoginAlertEnabled = user.IsLoginAlertEnabled;
			Name = user.Name;
        }
		
	    //public new string Id { get; set; }
	    
    }
}