using MyReptileFamilyLibrary.Builder;
using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyLibrary.Abstractions;

/// <summary>
///     Service for sending e-mail.
///     Active when <see cref="BuilderBase{TBuilder,THost}.WithEmailService" /> is called in setup.
/// </summary>
public interface IEmailService
{
    /// <summary>
    ///     Send an e-mail
    /// </summary>
    /// <param name="_p_To">The recipient of the e-mail</param>
    /// <param name="_p_From">The sender of the e-mail</param>
    /// <param name="_p_Subject">The subject of the e-mail</param>
    /// <param name="_p_PlainTextContent">The content of the e-mail</param>
    /// <param name="_p_CancellationToken">The cancellation token to observe</param>
    /// <returns>
    ///     True when the e-mail was sent without error; false otherwise
    /// </returns>
    Task<bool> SendEmailAsync(string _p_To, string _p_From, string _p_Subject, string _p_PlainTextContent, CancellationToken _p_CancellationToken);

    /// <summary>
    ///     Send an e-mail
    /// </summary>
    /// <param name="_p_Email">The e-mail to send</param>
    /// <param name="_p_CancellationToken">The cancellation token to observe</param>
    /// <returns>
    ///     True when the e-mail was sent without error; false otherwise
    /// </returns>
    Task<bool> SendEmailAsync(Email _p_Email, CancellationToken _p_CancellationToken);
}