namespace EcoCentre.Models.Domain.User.Commands
{
    public enum PasswordResetCommandResult
    {
        Success, UserNotFound, NoEmail
    }
}