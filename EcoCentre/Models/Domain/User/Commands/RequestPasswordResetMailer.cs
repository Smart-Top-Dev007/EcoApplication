using Mvc.Mailer;

namespace EcoCentre.Models.Domain.User.Commands
{
    public class RequestPasswordResetMailer : MailerBase
    {
        public MvcMailMessage Create(User user)
        {
            ViewBag.EmailResetKey = user.EmailResetKey;
            return Populate(x =>
                {
                    x.ViewName = "~/Views/MailTemplates/ResetEmail.cshtml";
                    x.To.Add(user.Email);
                    x.Subject = "EcoCentre - Mot de passe réinitialisé";
                });
        }
    }
}