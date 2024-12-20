using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="DateOnly"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class DateOnlySqlParameterExtensions
{
    /// <summary>
    /// Converts a DateOnly object to a SqlParameter
    /// </summary>
    /// <param name="_p_Value"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this DateOnly _p_Value, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Date)
        => new(_p_Name, _p_DbType) { Value = _p_Value };

    /// <summary>
    /// Converts a Nullable DateOnly object to a SqlParameter.  Null values will be inserted with DBNull.Value.
    /// </summary>
    /// <param name="_p_Value"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this DateOnly? _p_Value, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Date)
        => new(_p_Name, _p_DbType) { Value = _p_Value.HasValue ? _p_Value.Value : DBNull.Value };
}
