using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyLibrary.SQL.Abstractions;

/// <summary>
///     SQL to be executed against a database.
///     Use <see cref="IDapperQuery{TReturnType}" /> to create queries that return data selected from the database.
/// </summary>
public interface IDapperSQL
{
    /// <summary>
    ///     The SQL text with parameters declared as @ParameterName.
    /// </summary>
    string SQL { get; }

    /// <summary>
    ///     The parameters that will populate the provided <see cref="SQL" /> with.
    /// </summary>
    DapperParameter DapperParameter { get; }
}