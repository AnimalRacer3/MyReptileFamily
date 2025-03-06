using System.Data;
using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

/// <summary>
///     General extension methods for <see cref="MySqlParameter" />
/// </summary>
public static class SqlParameterBuildExtensions
{
    /// <summary>
    ///     Uses <see cref="MySqlParameter" />s to build an SQL INSERT statement for <paramref name="TableName" />
    /// </summary>
    public static string BuildInsertSQL(this List<MySqlParameter> Parameters, string TableName)
    {
        string _columns = string.Join($"{Environment.NewLine}    , ",
            Parameters.Select(Parameter => Parameter.ParameterName[1..]));
        string _values = string.Join($"{Environment.NewLine}    , ",
            Parameters.Select(Parameter => Parameter.ParameterName));
        return $"""
                INSERT INTO {TableName}
                (
                      {_columns}
                )
                VALUES
                (
                      {_values}
                );
                """;
    }

    /// <summary>
    ///     Returns a single string containing every input parameter's name and value.
    /// </summary>
    public static string BuildInputStringList(this List<MySqlParameter> Parameters)
    {
        List<ParameterDirection> _parametersToList = [ParameterDirection.Input, ParameterDirection.InputOutput];
        return string.Join(", ", Parameters
            .Where(Parameter => _parametersToList.Contains(Parameter.Direction))
            .Select(Parameter => $"{Parameter.ParameterName} [{Parameter.Value}]"));
    }
}