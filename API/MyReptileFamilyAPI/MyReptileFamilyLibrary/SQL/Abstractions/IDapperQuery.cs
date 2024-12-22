using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL.Abstractions;

/// <summary>
///     A SQL query and parameter collection that will use Dapper to query the database with.
///     Use <see cref="IDapperSQL" /> to create SQL operations that aren't queries (INSERT, UPDATE, etc.)
/// </summary>
/// <typeparam name="TReturnType">The type of object that we are going to SELECT and return from the database</typeparam>
public interface IDapperQuery<TReturnType>
{
    /// <summary>
    ///     The SQL text with parameters declared as @ParameterName.
    /// </summary>
    string SQL { get; }

    /// <summary>
    ///     The parameters that will populate the provided <see cref="SQL" /> with.
    /// </summary>
    List<MySqlParameter> Parameters { get; }
}