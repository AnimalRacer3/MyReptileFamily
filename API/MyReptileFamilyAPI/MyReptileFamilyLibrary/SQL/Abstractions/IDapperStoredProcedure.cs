using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyLibrary.SQL.Abstractions;

/// <summary>
///     Represents a stored procedure.
/// </summary>
public interface IDapperStoredProcedure
{
    /// <summary>
    ///     The name of the Stored Procedure to call.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Parameters to pass to the given procedure.
    /// </summary>
    DapperParameter DapperParameter { get; }
}