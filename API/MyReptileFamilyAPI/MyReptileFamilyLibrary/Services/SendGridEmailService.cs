using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Records;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyReptileFamilyLibrary.Services;

/// <summary>
///     An implementation of <see cref="IEmailService" /> that uses SendGrid to send e-mail.
/// </summary>
/// <remarks>
///     https://www.twilio.com/blog/send-emails-using-the-sendgrid-api-with-dotnetnet-6-and-csharp
/// </remarks>
internal class SendGridEmailService(
    ILogger<SendGridEmailService> Logger,
    ISendGridSettings Settings,
    IServiceProvider ServiceProvider)
    : EmailServiceBase(Logger)
{
    protected override ISendGridSettings EmailServiceSettings => Settings;

    protected override async Task<bool> SendEmail(Email Email, CancellationToken CancellationToken)
    {
        if (Email.To.Count(email => !IsDomainValid(email)) > 0)
        {
            Logger.LogWarning("[{Service}] Email provided does not exist", nameof(SendGridEmailService));
            return false;
        }
        
        try
        {
            SendGridMessage _msg = new()
            {
                From = new EmailAddress(Email.From),
                Subject = Email.Subject,
                PlainTextContent = Email.PlainTextContent
            };
            foreach (string _to in Email.To) _msg.AddTo(new EmailAddress(_to));
            foreach (string _cc in Email.Cc) _msg.AddCc(new EmailAddress(_cc));
            foreach (string _bcc in Email.Bcc) _msg.AddBcc(new EmailAddress(_bcc));

            // We inject IServiceProvider instead of ISendGridClient in the constructor, since these
            // classes essentially work as singletons in long-running Windows Services. ISendGridClient has
            // an issue in this scenario where it constantly recycles its underlying HttpClient, which floods
            // the logs. We get an instance every time SendEmail() is called instead to prevent this issue.
            ISendGridClient _sendGridClient = ServiceProvider.GetRequiredService<ISendGridClient>();
            Response? _response = await _sendGridClient.SendEmailAsync(_msg, CancellationToken);
            return _response.IsSuccessStatusCode;
        }
        catch (Exception _ex)
        {
            Logger.LogError(_ex, "[{Service}] Error occurred when attempting to send an e-mail",
                nameof(SendGridEmailService));
            return false;
        }
    }
}