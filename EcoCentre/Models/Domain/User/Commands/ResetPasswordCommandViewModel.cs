namespace EcoCentre.Models.Domain.User.Commands
{
    public class ResetPasswordCommandViewModel
    {
        public string Key { get; set; }
        public string Password { get; set; }
        public bool Reseting { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}