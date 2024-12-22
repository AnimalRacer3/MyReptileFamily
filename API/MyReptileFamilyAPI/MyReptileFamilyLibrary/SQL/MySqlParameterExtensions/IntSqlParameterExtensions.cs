using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="int"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class IntSqlParameterExtensions
{
    /// <summary>
    /// Converts an int to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Int"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this int _p_Int, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int32)
        => new(_p_Name, _p_DbType) { Value = _p_Int };

    /// <summary>
    /// Converts an int to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Int"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this int? _p_Int, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int32)
        => new(_p_Name, _p_DbType) { Value = _p_Int ?? (object) DBNull.Value };
}
