using System.Data;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MySql.Data.MySqlClient;

namespace MyReptileFamilyLibrary.SQL;

/// <summary>
///     Repository exposing access to various Dapper query/SQL operations.
///     Can inherit to create custom/bundled repository methods, i.e. tasks
///     that require several queries/SQL operations to occur in a row or in
///     a single transaction.
///     set up in Program.cs.
/// </summary>
public interface IMRFRepository
{
    /// <summary>
    ///     Creates a <see cref="MySqlConnection" /> instance. Best used in a using statement
    ///     (e.g. "await using SqlConnection _connection = CreateSqlConnection();").
    /// </summary>
    IMySQLConnection CreateMySQLConnection(string ConnectionString);

    /// <summary>
    ///     Run basic queries against the database
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<List<TReturnType>> QueryAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Run a query and return the first result.
    ///     Results in an exception if no records found.
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<TReturnType> QueryFirstAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Run a query and return the first result (or default if nothing found)
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<TReturnType?> QueryFirstOrDefaultAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Run a query and return the sole result.
    ///     Results in an exception unless exactly one record is found.
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<TReturnType> QuerySingleAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Run a query and return the sole result, or null if nothing found.
    ///     Results in an exception unless exactly 0 or 1 record is found.
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<TReturnType> QuerySingleOrDefaultAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Executes a Scalar Query against the database
    /// </summary>
    /// <param name="Query">The query to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<TReturnType?> ExecuteScalarAsync<TReturnType>(IDapperQuery<TReturnType> Query, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Execute arbitrary SQL against the database
    /// </summary>
    /// <param name="SQL">The SQL to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task<int> ExecuteAsync(IDapperSQL SQL, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Execute a given Stored Procedure
    /// </summary>
    /// <param name="StoredProcedure">The StoredProcedure to execute</param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="Transaction">The active SQL transaction</param>
    Task ExecuteStoredProcedureAsync(IDapperStoredProcedure StoredProcedure, IMySQLConnection Connection,
        IDbTransaction? Transaction = null);

    /// <summary>
    ///     Executes the provided function within the scope of a SQL Transaction.
    ///     If an exception is thrown, the Transaction is rolled back (and the exception re-thrown).
    ///     If no errors occur, then the Transaction is committed.
    /// </summary>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="DapperTransaction">Contains all the SQL to run within the Transaction.</param>
    /// <param name="CancellationToken">The cancellation token.</param>
    /// <remarks>
    ///     Stored procedures that do their own Transaction management should not use this method, as conflicts
    ///     could arise with multiple Transactions taking place.
    /// </remarks>
    Task ExecuteTransactionAsync(IDapperTransaction DapperTransaction, IMySQLConnection Connection,
        CancellationToken CancellationToken);

    /// <summary>
    ///     Executes the provided function within the scope of a SQL Transaction, where the function
    ///     of the Transaction returns a value.
    ///     If an exception is thrown, the Transaction is rolled back (and the exception re-thrown).
    ///     If no errors occur, then the Transaction is committed.
    /// </summary>
    /// <param name="DapperTransaction">Contains all the SQL to run within the Transaction, that returns a value. </param>
    /// <param name="Connection">The active SQL connection; call <see cref="CreateMySQLConnection"/> to create one</param>
    /// <param name="CancellationToken">The cancellation token.</param>
    /// <remarks>
    ///     Stored procedures that do their own Transaction management should not use this method, as conflicts
    ///     could arise with multiple Transactions taking place.
    /// </remarks>
    /// <returns>The value specified in the SQL function called.</returns>
    Task<TReturnType> ExecuteTransactionAsync<TReturnType>(IDapperTransaction<TReturnType> DapperTransaction,
        IMySQLConnection Connection, CancellationToken CancellationToken);
}