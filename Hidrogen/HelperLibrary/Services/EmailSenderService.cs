using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Microsoft.Extensions.Logging;

namespace HelperLibrary.Services {

    public class EmailSenderService : IEmailSenderService {

        private readonly ILogger<EmailSenderService> _logger;
        private readonly SmtpClient _emailPusher;
        private MailMessage _message;

        public EmailSenderService(
            ILogger<EmailSenderService> logger
        ) {
            _logger = logger;

            //Access for less secure app has been turn on in gmail setting
            _emailPusher = new SmtpClient {
                Host = HidroConstants.MAIL_SERVER_HOST,
                Port = HidroConstants.MAIL_SERVER_PORT,
                EnableSsl = HidroConstants.MAIL_SERVER_TLS,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = HidroConstants.USE_DEFAULT_CREDENTIALS,
                Credentials = new NetworkCredential(
                    HidroConstants.MAIL_SERVER_USERNAME,
                    HidroConstants.MAIL_SERVER_PASSWORD
                )
            };
            _message = new MailMessage();
        }

        public async Task<bool> SendEmail(EmailParamVM message) {
            _logger.LogInformation("EmailSenderService.SendEmail - Service starts.");

            try {
                ComposeEmail(message);

                await _emailPusher.SendMailAsync(_message);
                _message = new MailMessage();
            } catch (Exception ex) {
                _logger.LogError("EmailSenderService.SendEmail - Error: " + ex);
                return false;
            }

            return true;
        }

        public async Task<bool> SendEmails(List<EmailParamVM> messages) {
            _logger.LogInformation("EmailSenderService.SendEmails - Service starts.");

            try {
                foreach (var message in messages) {
                    ComposeEmail(message);
                    await _emailPusher.SendMailAsync(_message);
                    _message = new MailMessage();
                }
            } catch (Exception ex) {
                _logger.LogError("EmailSenderService.SendEmail - Error: " + ex);
                return false;
            }

            return true;
        }

        private void ComposeEmail(EmailParamVM message) {
            _message.From = new MailAddress(
                string.IsNullOrEmpty(message.SenderAddress) ? HidroConstants.MAIL_SENDER_ADDRESS : message.SenderAddress,
                string.IsNullOrEmpty(message.SenderName) ? HidroConstants.MAIL_SENDER_NAME : message.SenderName
            );

            _message.To.Add(new MailAddress(message.ReceiverAddress, message.ReceiverName));

            _message.Subject = message.Subject;

            _message.IsBodyHtml = true;
            _message.Body = message.Body;

            if (message.Attachments != null)
                foreach (var attachment in message.Attachments)
                    _message.Attachments.Add(new Attachment(attachment));
        }
    }
}
