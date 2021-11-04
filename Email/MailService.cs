using Domain.Email;
using Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mail
{
    public class MailService : IMailService
    {
        private readonly EmailSettings _emailSettings;
        public MailService(IOptions<EmailSettings> emailSettings) {
            _emailSettings = emailSettings.Value;
        }

        public async Task EmailChangePassword(EmailRequest emailRequest) {
            emailRequest.Body = $"<p style='text-align: center; width: 250px; height: 50px; padding: 5px; border-radius: 20%;color: white;background: blue;text-decoration: none;'>" +
                $" Su nueva contraseña es : <strong>{emailRequest.Body}</strong></p> ";
            await SendEmailAsync(emailRequest);
        }

        public async Task EmailConfirmRegister(EmailRequest emailRequest) {
            emailRequest.Body = $"<a href='{emailRequest.Body}' target='_blank' " +
                $"style='width: 250px; height: 120px; padding: 5px; border-radius: 20%;color: white;background: blue;text-decoration: none;'" +
                $"> Confirmar Registro </a>";
            await SendEmailAsync(emailRequest);
        }

        public async Task SendEmailAsync(EmailRequest emailRequest) {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            message.From = new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName);
            message.To.Add(new MailAddress(emailRequest.ToEmail));
            message.Subject = emailRequest.Subject;

            message.IsBodyHtml = true;
            message.Body = emailRequest.Body;

            smtp.Port = _emailSettings.Port;
            smtp.Host = _emailSettings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_emailSettings.Mail, _emailSettings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }
    }
}
