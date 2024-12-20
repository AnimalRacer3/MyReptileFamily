using System.Data;
using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="bool"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class BoolSqlParameterExtensions
{
    /// <summary>
    /// Converts a bool to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Bool"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Bit"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this bool _p_Bool, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Bit)
        => new(_p_Name, _p_DbType) { Value = _p_Bool };

    /// <summary>
    /// Converts a bool to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Bool"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Bit"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this bool? _p_Bool, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Bit)
        => new(_p_Name, _p_DbType) { Value = _p_Bool ?? (object) DBNull.Value };
}
