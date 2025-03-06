using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="char" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class CharSqlParameterExtensions
{
    /// <summary>
    ///     Converts a char to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Char">The char to turn into a parameter</param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.VarChar" /> by default</param>
    /// <param name="TreatWhiteSpaceAsNull">When true, whitespace characters will become <see cref="DBNull.Value" /></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this char Char, string Name,
        MySqlDbType DbType = MySqlDbType.VarChar, bool TreatWhiteSpaceAsNull = false)
    {
        return ToSqlParameter((char?)Char, Name, DbType, TreatWhiteSpaceAsNull);
    }

    /// <summary>
    ///     Converts a char to a <see cref="MySqlParameter" />
    /// </summary>
    /// <param name="Char">The char to turn into a parameter (nulls will become <see cref="DBNull.Value" />)</param>
    /// <param name="Name"></param>
    /// <param name="DbType"><see cref="MySqlDbType.VarChar" /> by default</param>
    /// <param name="TreatWhiteSpaceAsNull">When true, whitespace characters will become <see cref="DBNull.Value" /></param>
    /// <returns></returns>
    public static MySqlParameter ToSqlParameter(this char? Char, string Name,
        MySqlDbType DbType = MySqlDbType.VarChar, bool TreatWhiteSpaceAsNull = false)
    {
        return new MySqlParameter(Name, DbType, 1)
        {
            Value = Char.HasValue && (!TreatWhiteSpaceAsNull || !char.IsWhiteSpace(Char.Value))
                ? Char
                : DBNull.Value
        };
    }

    /// <summary>
    ///     Provides a list of SQL Parameters with indexed names, one for each item in the list.
    ///     Example: @Name0, @Name1
    ///     When building the SQL operation, use <see cref="ListSqlParameterExtensions.GetInClauseForList{T}" /> to add
    ///     "IN (@Name0, @Name1)" for you automatically.
    /// </summary>
    /// <returns>A comma-separated list of values, even when an empty list is given</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static List<MySqlParameter> ToSqlParametersForWhereInClause(this List<char> Values, string Name,
        MySqlDbType DbType = MySqlDbType.VarChar, bool TreatWhiteSpaceAsNull = false)
    {
        return Values.ToSqlParametersForWhereInClauseInternal(Char =>
            Char.ToSqlParameter(Name, DbType, TreatWhiteSpaceAsNull));
    }
}