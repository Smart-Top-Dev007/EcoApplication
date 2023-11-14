namespace EcoCentre.Models.Domain.User.Commands
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PasswordConfirmation { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsGlobalAdmin { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsAgent { get; set; }
        public bool IsLoginAlertEnabled { get; set; }
	    public string Name { get; set; }
	}

}