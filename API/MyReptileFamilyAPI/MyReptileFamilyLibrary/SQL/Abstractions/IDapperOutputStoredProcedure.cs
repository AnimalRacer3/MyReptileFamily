using MyReptileFamilyLibrary.Records;

namespace MyReptileFamilyLibrary.SQL.Abstractions;

/// <summary>
///     Represents a stored procedure that defines an Output Variable.
/// </summary>
/// <typeparam name="TOutputType">The data type of the output variable.</typeparam>
public interface IDapperOutputStoredProcedure<TOutputType>
{
    /// <summary>
    ///     The name of the stored procedure
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The name of the parameter whose value will be returned after executing this stored procedure
    /// </summary>
    string OutputParameterName { get; }

    /// <summary>
    ///     The parameters of the stored procedure
    /// </summary>
    DapperParameter DapperParameter { get; }
}