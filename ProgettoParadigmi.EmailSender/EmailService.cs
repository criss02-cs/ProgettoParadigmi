using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProgettoParadigmi.EmailSender
{
    public class EmailService
    {
        private SendGridClient _client;
        private EmailAddress _from;

        private const string INVITO_TEMPLATE = "d-e395edf8e0bd4319a5683adc7732a14f";
        private const string REMINDER_TEMPLATE = "d-8f6064230a484070bac601088114860a";

        public EmailService(IConfiguration configuration)
        {
            Console.WriteLine(configuration);
            var apiKey = configuration["SendGrid:ApiKey"];
            _client = new SendGridClient(apiKey);
            _from = new EmailAddress("cristianoaloigi0@gmail.com", "Cristiano Aloigi (admin)");
        }

        public async Task<bool> InviaEmailReminder(List<ReminderEmailDto> reminderEmails)
        {
            var message = new SendGridMessage()
            {
                From = _from,
                TemplateId = REMINDER_TEMPLATE,
                Personalizations = reminderEmails.Select(x => new Personalization
                {
                    Tos = x.Emails
                        .Select(y => new EmailAddress(y.Email, $"{y.Nome} ${y.Cognome}"))
                        .ToList(),
                    TemplateData = new
                    {
                        data = x.Data,
                        ora = x.Ora,
                        senderName = x.Organizzatore
                    }
                }).ToList()
            };
            var response = await _client.SendEmailAsync(message);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> InviaEmailInvito(List<EmailToDto> tosEmail, string from, DateTime dateEvent)
        {
            var tos = tosEmail.Select(x => new EmailAddress(x.Email)).ToList();
            var message = new SendGridMessage
            {
                From = _from,
                TemplateId = INVITO_TEMPLATE,
                Personalizations = tosEmail.Select(x => new Personalization
                {
                    Tos = new List<EmailAddress> { new(x.Email, $"{x.Nome} {x.Cognome}") },
                    TemplateData = new
                    {
                        data = dateEvent.Date.ToString("D"),
                        ora = dateEvent.ToString("HH:mm"),
                        senderName = from,
                        toName = $"{x.Nome} {x.Cognome}"
                    }
                }).ToList()
            };
            var response = await _client.SendEmailAsync(message);
            return response.IsSuccessStatusCode;
        }
    }

    public record EmailToDto(string Nome, string Cognome, string Email);

    public record ReminderEmailDto(string Organizzatore, string Data, string Ora, List<EmailToDto> Emails);
}
