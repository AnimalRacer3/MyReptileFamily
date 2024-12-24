namespace MyReptileFamilyLibrary.AppSettings;

/// <summary>
///     Settings used when interacting with the SendGrid API
/// </summary>
public interface ISendGridSettings
{
    /// <summary>
    ///     API Key used for SendGrid
    /// </summary>
    public string SendGridApiKey { get; set; }

    /// <summary>
    ///     E-mail will only be sent if this is true
    /// </summary>
    public bool EmailEnabled { get; set; }

    /// <summary>
    ///     When true, e-amil messages will be logged in addition to being e-mailed
    /// </summary>
    public bool LogEmails { get; set; }
}