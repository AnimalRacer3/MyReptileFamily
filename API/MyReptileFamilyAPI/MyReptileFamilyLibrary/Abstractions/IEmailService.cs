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
    /// <param name="To">The recipient of the e-mail</param>
    /// <param name="From">The sender of the e-mail</param>
    /// <param name="Subject">The subject of the e-mail</param>
    /// <param name="PlainTextContent">The content of the e-mail</param>
    /// <param name="CancellationToken">The cancellation token to observe</param>
    /// <returns>
    ///     True when the e-mail was sent without error; false otherwise
    /// </returns>
    Task<bool> SendEmailAsync(string To, string From, string Subject, string PlainTextContent,
        CancellationToken CancellationToken);

    /// <summary>
    ///     Send an e-mail
    /// </summary>
    /// <param name="Email">The e-mail to send</param>
    /// <param name="CancellationToken">The cancellation token to observe</param>
    /// <returns>
    ///     True when the e-mail was sent without error; false otherwise
    /// </returns>
    Task<bool> SendEmailAsync(Email Email, CancellationToken CancellationToken);

    /// <summary>
    /// Checks if the domain of the address given is a valid domain.
    /// </summary>
    /// <param name="Email">The e-mail to send</param>
    /// <returns>
    /// True when the e-mail has a valid domain; false otherwise
    /// </returns>
    bool IsDomainValid(string Email);
}