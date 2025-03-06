using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     Extensions to turn <see cref="List{T}" />s into <see cref="MySqlParameter" />s
/// </summary>
public static class ListSqlParameterExtensions
{
    /// <summary>
    ///     Generates "ColumnName IN (ParameterName0, ParameterName1)" part of SQL operation.
    ///     When needing <see cref="MySqlParameter" />s, call "_yourList.ToSqlParametersForWhereInClause()" />
    /// </summary>
    /// <param name="Values">
    ///     The list of values that will be added as <see cref="MySqlParameter" />s
    /// </param>
    /// <param name="ColumnName"> The name of the SQL table column being checked against </param>
    /// <param name="ParameterName">The name of the SQL parameter. Include the '@' at the beginning!</param>
    /// <returns>String to add to the SQL operation</returns>
    /// <exception cref="ArgumentException">Thrown when given empty list (at least one is required)</exception>
    public static string GetInClauseForList<T>(this List<T> Values, string ColumnName, string ParameterName)
    {
        if (Values.Count == 0) throw new ArgumentException("Must provide at least one value", nameof(Values));

        IEnumerable<string> _parameters = Values.Select((_, Index) => $"{ParameterName}{Index}");
        return $"{ColumnName} IN ({string.Join(", ", _parameters)})";
    }

    internal static List<MySqlParameter> ToSqlParametersForWhereInClauseInternal<T>(this List<T> Values,
        Func<T, MySqlParameter> GetSqlParameterFunc)
    {
        if (Values.Count == 0) throw new ArgumentException("Must provide at least one value", nameof(Values));

        return Values
            .Select((Value, Index) =>
            {
                MySqlParameter _sqlParameter = GetSqlParameterFunc(Value);
                _sqlParameter.ParameterName += Index;
                return _sqlParameter;
            })
            .ToList();
    }
}