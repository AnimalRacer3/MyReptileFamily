using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="Enum"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class EnumSqlParameterExtensions
{
    /// <summary>
    /// Converts an enum to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Enum">The char to turn into a parameter (nulls will become <see cref="DBNull.Value"/>)</param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.Enum"/> by default</param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this Enum? _p_Enum, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Enum)
    => new(_p_Name, _p_DbType) { Value = _p_Enum ?? (object)DBNull.Value };
}