using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="long"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class LongSqlParameterExtensions
{
    /// <summary>
    /// Converts a long (Int64) to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Long"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int64"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this long _p_Long, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int64)
        => new(_p_Name, _p_DbType) { Value = _p_Long };

    /// <summary>
    /// Converts a long (Int64) to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Long"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int64"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this long? _p_Long, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int64)
        => new(_p_Name, _p_DbType) { Value = _p_Long ?? (object) DBNull.Value };
}
