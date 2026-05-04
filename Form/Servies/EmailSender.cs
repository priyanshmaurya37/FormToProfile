using System.Net;
using System.Net.Mail;

namespace Form.Servies
{
    public class EmailSender
    {
        // Replace these with your actual details
        private readonly string _senderEmail = "mauryapriyansh326@gmail.com";
        private readonly string _appPassword = "anyqoccjwldacfpx";

        public void SendMail(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage(_senderEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set to true if you want to send HTML
            };

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _appPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
