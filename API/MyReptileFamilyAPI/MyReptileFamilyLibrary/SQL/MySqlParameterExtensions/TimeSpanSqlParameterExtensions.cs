using MySqlConnector;
using System.Data;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="TimeSpan"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class TimeSpanSqlParameterExtensions
{
    /// <summary>
    /// Converts a TimeSpan object to a SqlParameter.
    /// </summary>
    /// <param name="_p_Value"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this TimeSpan _p_Value, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Time)
        => new(_p_Name, _p_DbType) { Value = _p_Value };

    /// <summary>
    /// Converts a Nullable TimeSpan object to a SqlParameter.  Null values will be inserted with DBNull.Value.
    /// </summary>
    /// <param name="_p_Value"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this TimeSpan? _p_Value, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Time)
        => new(_p_Name, _p_DbType) { Value = _p_Value.HasValue ? _p_Value.Value : DBNull.Value };
}
