using System.Net;
using System.Net.Mail;
using Company.DAL.Models;

namespace Company.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			// by Mail Server : Gmail

			//Smtp protocol

			var client = new SmtpClient("smtp.gmail.com", 587);  // el server elly hst5dmoo 
			client.EnableSsl = true; // hyshafro but there is no certificate so nothing change 

			client.Credentials = new NetworkCredential("ammaryasser2018a18@gmail.com", "pssklbosttusnwnl"); // El account elly b3t mno 

			client.Send("ammaryasser2018a18@gmail.com", email.To, email.Subject, email.Body);
		}

	}
}
