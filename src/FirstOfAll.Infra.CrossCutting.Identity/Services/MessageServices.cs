using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FirstOfAll.Infra.CrossCutting.Identity.Services
{
    public class AuthEmailMessageSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            try{
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress("iran.marley@gservicer.com.br", "Muhammad Hassan Tariq")
                };
                mail.To.Add(new MailAddress(email));
                
                mail.Subject = "Personal Management System - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient("smtp-messaging-idc.neture.com.br", 587))
                {
                    smtp.Credentials = new NetworkCredential("iran.marley@gservicer.com.br", "Gservicer1");
                    smtp.EnableSsl = false;
                    smtp.SendMailAsync(mail);
                }
            }
            catch{
                
            }

            return Task.FromResult(0);
        }
    }

    public class AuthSMSMessageSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
