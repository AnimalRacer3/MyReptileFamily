using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
/// Extensions to turn <see cref="char"/>s into <see cref="MySqlParameter"/>s
/// </summary>
public static class CharSqlParameterExtensions
{
    /// <summary>
    /// Converts a char to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Char">The char to turn into a parameter</param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.VarChar"/> by default</param>
    /// <param name="_p_TreatWhiteSpaceAsNull">When true, whitespace characters will become <see cref="DBNull.Value"/></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this char _p_Char, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.VarChar, bool _p_TreatWhiteSpaceAsNull = false)
        => ToSqlParameter((char?) _p_Char, _p_Name, _p_DbType, _p_TreatWhiteSpaceAsNull);

    /// <summary>
    /// Converts a char to a <see cref="MySqlParameter"/>
    /// </summary>
    /// <param name="_p_Char">The char to turn into a parameter (nulls will become <see cref="DBNull.Value"/>)</param>
    /// <param name="_p_Name"></param>
    /// <param name="_p_DbType"><see cref="MySqlDbType.VarChar"/> by default</param>
    /// <param name="_p_TreatWhiteSpaceAsNull">When true, whitespace characters will become <see cref="DBNull.Value"/></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this char? _p_Char, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.VarChar, bool _p_TreatWhiteSpaceAsNull = false)
    {
        return new(_p_Name, _p_DbType, 1)
        {
            Value = _p_Char.HasValue && (!_p_TreatWhiteSpaceAsNull || !char.IsWhiteSpace(_p_Char.Value))
                ? _p_Char
                : DBNull.Value
        };
    }

    /// <summary>
    /// Provides a list of SQL Parameters with indexed names, one for each item in the list.
    /// Example: @Name0, @Name1
    /// When building the SQL operation, use <see cref="ListSqlParameterExtensions.GetInClauseForList{T}"/> to add
    /// "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClause(this List<char> _p_Values, string _p_Name, MySqlDbType _p_DbType = MySqlDbType.VarChar, bool _p_TreatWhiteSpaceAsNull = false)
        => _p_Values.ToSqlParametersForWhereInClauseInternal(_p_Char => _p_Char.ToSqlParameter(_p_Name, _p_DbType, _p_TreatWhiteSpaceAsNull));
}
