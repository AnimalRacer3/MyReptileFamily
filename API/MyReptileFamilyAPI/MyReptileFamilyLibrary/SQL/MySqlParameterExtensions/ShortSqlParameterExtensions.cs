using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="short"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class ShortSqlParameterExtensions
{
    /// <summary>
    /// Converts a short to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Short"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int16"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this short _p_Short, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int16)
        => new(_p_Name, _p_DbType) { Value = _p_Short };

    /// <summary>
    /// Converts a short to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Short"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Int16"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this short? _p_Short, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Int16)
        => new(_p_Name, _p_DbType) { Value = _p_Short ?? (object) DBNull.Value };
}
