using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.Abstractions;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace MyReptileFamilyLibrary.Services;

/// <summary>
///     An implementation of <see cref="IEmailService"/> that uses SendGrid to send e-mail.
/// </summary>
/// <remarks>
///     https://www.twilio.com/blog/send-emails-using-the-sendgrid-api-with-dotnetnet-6-and-csharp
/// </remarks>
internal class SendGridEmailService(ILogger<SendGridEmailService> _p_Logger, ISendGridSettings _p_Settings, IServiceProvider _p_ServiceProvider)
    : EmailServiceBase(_p_Logger)
{
    protected override ISendGridSettings EmailServiceSettings => _p_Settings;

    protected override async Task<bool> SendEmail(Email _p_Email, CancellationToken _p_CancellationToken)
    {
        try
        {
            SendGridMessage _msg = new()
            {
                From = new EmailAddress(_p_Email.From),
                Subject = _p_Email.Subject,
                PlainTextContent = _p_Email.PlainTextContent
            };
            foreach (string _to in _p_Email.To) _msg.AddTo(new EmailAddress(_to));
            foreach (string _cc in _p_Email.CC) _msg.AddCc(new EmailAddress(_cc));
            foreach (string _bcc in _p_Email.BCC) _msg.AddBcc(new EmailAddress(_bcc));

            // We inject IServiceProvider instead of ISendGridClient in the constructor, since these
            // classes essentially work as singletons in long-running Windows Services. ISendGridClient has
            // an issue in this scenario where it constantly recycles its underlying HttpClient, which floods
            // the logs. We get an instance every time SendEmail() is called instead to prevent this issue.
            ISendGridClient _sendGridClient = _p_ServiceProvider.GetRequiredService<ISendGridClient>();
            Response _response = await _sendGridClient.SendEmailAsync(_msg, _p_CancellationToken);
            return _response.IsSuccessStatusCode;
        }
        catch (Exception _ex)
        {
            _p_Logger.LogError(_ex, "[{Service}] Error occurred when attempting to send an e-mail", nameof(SendGridEmailService));
            return false;
        }
    }
}