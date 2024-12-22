using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyLibrary.Services;

internal abstract class EmailServiceBase(ILogger _p_Logger) : IEmailService
{
    protected abstract ISendGridSettings EmailServiceSettings { get; }
    protected abstract Task<bool> SendEmail(Email _p_Email, CancellationToken _p_CancellationToken);

    public Task<bool> SendEmailAsync(string _p_To, string _p_From, string _p_Subject, string _p_PlainTextContent, CancellationToken _p_CancellationToken)
        => SendEmailAsync(new Email(_p_Subject, _p_PlainTextContent, _p_From, _p_To), _p_CancellationToken);

    public async Task<bool> SendEmailAsync(Email _p_Email, CancellationToken _p_CancellationToken)
    {
        if (EmailServiceSettings.LogEmails)
        {
            _p_Logger.LogDebug(_EMAIL_LOG_TEMPLATE, _p_Email.To, _p_Email.From, _p_Email.Subject, _p_Email.CC, _p_Email.BCC, _p_Email.PlainTextContent);
        }
        return !EmailServiceSettings.EmailEnabled || await SendEmail(_p_Email, _p_CancellationToken);
    }

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
}