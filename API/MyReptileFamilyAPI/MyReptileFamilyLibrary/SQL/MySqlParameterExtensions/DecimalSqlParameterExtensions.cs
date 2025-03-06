using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="decimal" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class DecimalSqlParameterExtensions
{
    /// <summary>
    ///     Create an sql parameter for a decimal
    /// </summary>
    /// <param name="Decimal"></param>
    /// <param name="Name"></param>
    /// <param name="Precision">Defaults to 0</param>
    /// <param name="Scale">Defaults to 0</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(
        this decimal Decimal,
        string Name,
        byte Precision = 0,
        byte Scale = 0)
    {
        return new MySqlParameter(Name, MySqlDbType.Decimal)
            { Precision = Precision, Scale = Scale, Value = Decimal };
    }

    /// <summary>
    ///     Create an sql parameter for a decimal
    /// </summary>
    /// <param name="Decimal"></param>
    /// <param name="Name"></param>
    /// <param name="Precision">Defaults to 0</param>
    /// <param name="Scale">Defaults to 0</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(
        this decimal? Decimal,
        string Name,
        byte Precision = 0,
        byte Scale = 0)
    {
        return new MySqlParameter(Name, MySqlDbType.Decimal)
            { Precision = Precision, Scale = Scale, Value = Decimal };
    }
}