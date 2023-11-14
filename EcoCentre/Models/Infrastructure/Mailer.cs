using System.IO;
using System.Net.Mail;

namespace EcoCentre.Models.Infrastructure
{
	public class Mailer
	{
		public void Send(string destinationAddress, string subject, string content)
		{
			using (var client = new SmtpClient())
			using (var mail = new MailMessage())
			{
				foreach (var address in destinationAddress.Split(';', ','))
				{
					mail.To.Add(address);
				}

				var templateFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/compo/mailTemplate.txt");
				var template = File.ReadAllText(templateFile);

				var body = template.Replace("{{body}}", content);

				mail.Subject = subject;
				mail.Body = body;
				mail.IsBodyHtml = true;
				client.Send(mail);
			}
		}
		public void SendPlain(string destinationAddress, string subject, string content)
		{
			using (var client = new SmtpClient())
			using (var mail = new MailMessage())
			{
				foreach (var address in destinationAddress.Split(';', ','))
				{
					mail.To.Add(address);
				}

				mail.Subject = subject;
				mail.Body = content;
				mail.IsBodyHtml = false;
				client.Send(mail);
			}
		}
	}
}