using System.Data;

namespace MyReptileFamilyLibrary.SQL.Abstractions;

/// <summary>
/// A defined series of queries or other SQL operations to be executed
/// within a given transaction
/// </summary>
public interface IDapperTransaction
{
    /// <summary>
    /// The SQL operations to run under the given transaction. Make sure to
    /// pass along the provided transaction with every SQL operation!
    /// </summary>
    Func<IMySQLConnection, IMRFRepository, IDbTransaction, Task> SQLFunc { get; }
}

/// <summary>
/// A defined series of queries or other SQL operations to be executed
/// within a given transaction, which ultimately returns something.
/// </summary>
/// <typeparam name="TReturnType">The type of object to return</typeparam>
public interface IDapperTransaction<TReturnType>
{
    /// <summary>
    /// The SQL operations to run under the given transaction. Make sure to
    /// pass along the provided transaction with every SQL operation!
    /// </summary>
    Func<IMySQLConnection, IMRFRepository, IDbTransaction, Task<TReturnType>> SQLFunc { get; }
}
