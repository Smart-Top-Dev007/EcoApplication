using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.ViewModel;
using Mvc.Mailer;

namespace EcoCentre.Models.Domain.Clients.Commands
{
    public class CustomAddressCreatedMailer: MailerBase
    {
        public MvcMailMessage Create(Client client, string hostUrl, string mailAddress)
        {
            ViewBag.client = client;
            ViewBag.hostUrl = hostUrl;
            return Populate(x =>
            {
                x.ViewName = "~/Views/MailTemplates/customAddressCreatedEmail.cshtml";
                x.To.Add(mailAddress);
                x.Subject = "Nouvelle adresse de l'utilisateur personnalisé a été créé";
            });
        }

		public MvcMailMessage Create1(Client1 client, string hostUrl, string mailAddress)
		{
			ViewBag.client = client;
			ViewBag.hostUrl = hostUrl;
			return Populate(x =>
			{
				x.ViewName = "~/Views/MailTemplates/customAddressCreatedEmail.cshtml";
				x.To.Add(mailAddress);
				x.Subject = "Nouvelle adresse de l'utilisateur personnalisé a été créé";
			});
		}
	}
}