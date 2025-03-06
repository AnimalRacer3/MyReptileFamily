namespace MyReptileFamilyLibrary.Records;

/// <summary>
///     Represents an e-mail to send
/// </summary>
/// <param name="Subject">The subject of the e-mail</param>
/// <param name="PlainTextContent">The content of the e-mail</param>
/// <param name="From">The sender of the e-mail</param>
/// <param name="To">The recipients of the e-mail</param>
public record Email(string Subject, string PlainTextContent, string From, params string[] To)
{
    /// <summary>
    ///     The carbon copy recipients of the e-mail
    /// </summary>
    public IEnumerable<string> Cc { get; set; } = [];

    /// <summary>
    ///     The blind carbon copy recipients of the e-mail
    /// </summary>
    public IEnumerable<string> Bcc { get; set; } = [];

    /// <summary>
    ///     The id of a template to be used
    /// </summary>
    public string TemplateId { get; set; } = "";
}