using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Records;
using System.Net;

namespace MyReptileFamilyLibrary.Services;

internal abstract class EmailServiceBase(ILogger Logger) : IEmailService
{
    private const string _EMAIL_LOG_TEMPLATE = """
                                               E-mail Log:
                                               =====
                                               To: {To}
                                               From: {From}
                                               Subject: "{Subject}"
                                               CC: {CC}
                                               BCC: {BCC}
                                               Contents:
                                               {Contents}
                                               =====
                                               """;

    protected abstract ISendGridSettings EmailServiceSettings { get; }

    public Task<bool> SendEmailAsync(string To, string From, string Subject, string PlainTextContent,
        CancellationToken CancellationToken)
    {
        return SendEmailAsync(new Email(Subject, PlainTextContent, From, To), CancellationToken);
    }

    public async Task<bool> SendEmailAsync(Email Email, CancellationToken CancellationToken)
    {
        if (Email.To.Count(EmailString => !IsDomainValid(EmailString)) > 0)
        {
            Logger.LogWarning("[{Service}] Email provided does not exist", nameof(SendGridEmailService));
            return false;
        }
        if (EmailServiceSettings.LogEmails)
            Logger.LogDebug(_EMAIL_LOG_TEMPLATE, Email.To, Email.From, Email.Subject, Email.Cc,
                Email.Bcc, Email.PlainTextContent);
        return !EmailServiceSettings.EmailEnabled || await SendEmail(Email, CancellationToken);
    }

    protected abstract Task<bool> SendEmail(Email Email, CancellationToken CancellationToken);

    public bool IsDomainValid(string Email)
    {
        try
        {
            string domain = Email.Split('@')[1];
            IPHostEntry hostEntry = Dns.GetHostEntry(domain); // Checks if the domain resolves
            return hostEntry.AddressList.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}