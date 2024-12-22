using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="decimal"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class DecimalSqlParameterExtensions
{
    /// <summary>
    ///  Create an sql parameter for a decimal
    /// </summary>
    /// <param name="_p_Decimal"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_Precision">Defaults to 0</param>
    /// <param name="_p_Scale">Defaults to 0</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(
        this decimal _p_Decimal,
        string _p_Name,
        byte _p_Precision = 0,
        byte _p_Scale = 0)
    {
        return new(_p_Name, MySqlDbType.Decimal) { Precision = _p_Precision, Scale = _p_Scale, Value = _p_Decimal };
    }

    /// <summary>
    ///  Create an sql parameter for a decimal
    /// </summary>
    /// <param name="_p_Decimal"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_Precision">Defaults to 0</param>
    /// <param name="_p_Scale">Defaults to 0</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(
        this decimal? _p_Decimal,
        string _p_Name,
        byte _p_Precision = 0,
        byte _p_Scale = 0)
    {
        return new(_p_Name, MySqlDbType.Decimal) { Precision = _p_Precision, Scale = _p_Scale, Value = _p_Decimal };
    }
}
