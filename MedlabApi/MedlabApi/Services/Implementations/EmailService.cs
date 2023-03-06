using MedlabApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace MedlabApi.Services.Implementations
{
    public  class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMail(string To, string subject, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("tahirtahirli2002@gmail.com", _configuration.GetSection("GoogleAuth:AppPassword").Value);
            smtpClient.EnableSsl = true;

            // message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tahirtahirli2002@gmail.com");
            mailMessage.To.Add(To);
            mailMessage.Subject = subject;
            mailMessage.Body = message;


            smtpClient.Send(mailMessage);
        }
    }
}
