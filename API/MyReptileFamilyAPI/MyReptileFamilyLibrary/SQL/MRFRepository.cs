using MyReptileFamilyLibrary.SQL.Abstractions;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Data;
using System.Data.Common;
using MyReptileFamilyLibrary.AppSettings;
using MySqlConnector;

namespace MyReptileFamilyLibrary.SQL;

public class MRFRepository(ILogger<MRFRepository> Logger, IMySQLConnectionString _p_Settings) : IMRFRepository
{
    private readonly string _defaultConnectionString = _p_Settings.MySQLConnectionString;
    /// <inheritdoc />
    public IMySQLConnection CreateMySQLConnection(string? SQLConnectionString) => 
        new MySQLConnectionWrapper(new MySqlConnection(SQLConnectionString ?? _defaultConnectionString));

    /// <inheritdoc />
    public async Task<List<TReturnType>> QueryAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] QueryAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        var _queryResult = await SQLConnection.QueryAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
        return _queryResult.ToList();
    }

    /// <inheritdoc />
    public async Task<TReturnType> QueryFirstAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] QueryFirstAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        var _queryResult = await SQLConnection.QueryFirstAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
        return _queryResult;
    }

    /// <inheritdoc />
    public async Task<TReturnType?> QueryFirstOrDefaultAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] QueryFirstOrDefaultAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        var _queryResult = await SQLConnection.QueryFirstOrDefaultAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
        return _queryResult ?? default;
    }

    /// <inheritdoc />
    public async Task<TReturnType> QuerySingleAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] QuerySingleAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        var _queryResult = await SQLConnection.QuerySingleAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
        return _queryResult;
    }

    /// <inheritdoc />
    public async Task<TReturnType?> QuerySingleOrDefaultAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] QuerySingleOrDefaultAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        var _queryResult = await SQLConnection.QuerySingleOrDefaultAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
        return _queryResult ?? default;
    }

    /// <inheritdoc />
    public async Task<TReturnType?> ExecuteScalarAsync<TReturnType>(IDapperQuery<TReturnType> Query,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] ExecuteScalarAsync: {Query}", nameof(MRFRepository), Query.GetType().Name);
        return await SQLConnection.ExecuteScalarAsync<TReturnType>(Query.SQL, Query.Parameters, Transaction);
    }

    /// <inheritdoc />
    public async Task<int> ExecuteAsync(IDapperSQL SQL,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] ExecuteAsync: {SQL}", nameof(MRFRepository), SQL.GetType().Name);
        return await SQLConnection.ExecuteAsync(SQL.SQL, SQL.Parameters, Transaction);
    }

    /// <inheritdoc />
    public async Task ExecuteStoredProcedureAsync(IDapperStoredProcedure StoredProcedure,
        IMySQLConnection SQLConnection, IDbTransaction? Transaction = null)
    {
        Logger.LogDebug("[{Repository}] ExecuteStoredProcedureAsync: {Procedure}", nameof(MRFRepository), StoredProcedure.GetType().Name);
        await SQLConnection.ExecuteAsync(StoredProcedure.Name, StoredProcedure.Parameters,
            commandType: CommandType.StoredProcedure, transaction: Transaction);
    }

    /// <inheritdoc />
    public async Task ExecuteTransactionAsync(IDapperTransaction DapperTransaction,
        IMySQLConnection SQLConnection, CancellationToken CancellationToken)
    {
        string _transactionName = $"{DapperTransaction.GetType().Name}-{Guid.NewGuid()}";
        await using DbTransaction _transaction = await SQLConnection.BeginDbTransactionAsync();
        try
        {
            Logger.LogDebug("[{Repository}] Beginning transaction [{Transaction}]", nameof(MRFRepository), _transactionName);
            await DapperTransaction.SQLFunc(SQLConnection, this, _transaction);
            await _transaction.CommitAsync(CancellationToken);
            Logger.LogDebug("[{Repository}] Committed transaction [{Transaction}]", nameof(MRFRepository), _transactionName);
        }
        catch (Exception _ex)
        {
            await _transaction.RollbackAsync(CancellationToken);
            Logger.LogError(_ex, "[{Repository}] Transaction [{Transaction}] rolled back due to error", nameof(MRFRepository), _transactionName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<TReturnType> ExecuteTransactionAsync<TReturnType>(IDapperTransaction<TReturnType> DapperTransaction,
        IMySQLConnection SQLConnection, CancellationToken CancellationToken)
    {
        string _transactionName = $"{DapperTransaction.GetType().Name}-{Guid.NewGuid()}";
        await using DbTransaction _transaction = await SQLConnection.BeginDbTransactionAsync();
        try
        {
            Logger.LogDebug("[{Repository}] Beginning transaction [{Transaction}]", nameof(MRFRepository), _transactionName);
            var _returnValue = await DapperTransaction.SQLFunc(SQLConnection, this, _transaction);
            await _transaction.CommitAsync(CancellationToken);
            Logger.LogDebug("[{Repository}] Committed transaction [{Transaction}]", nameof(MRFRepository), _transactionName);
            return _returnValue;
        }
        catch (Exception _ex)
        {
            await _transaction.RollbackAsync(CancellationToken);
            Logger.LogError(_ex, "[{Repository}] Transaction [{Transaction}] rolled back due to error", nameof(MRFRepository), _transactionName);
            throw;
        }
    }
}