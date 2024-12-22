using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="Guid"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class GuidSqlParameterExtensions
{
    /// <summary>
    /// Converts a GUID to a SQL Parameter
    /// </summary>
    /// <param name="_p_Guid"></param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this Guid _p_Guid, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.Guid)
        => new(_p_Name, _p_DbType) { Value = _p_Guid };
}
