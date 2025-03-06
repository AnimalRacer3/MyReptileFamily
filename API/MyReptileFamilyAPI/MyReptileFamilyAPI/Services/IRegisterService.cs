using MyReptileFamilyAPI.Models;
using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyAPI.Services;

public interface IRegisterService
{
    Task<IResult> ValidateOwnerWithDatabaseAsync(RegisterOwner Owner, IMySQLConnection SqlConn, CancellationToken CancellationToken);
    Task CreateValidationAndWelcomeEmailAsync(RegisterOwner Owner, IMySQLConnection SqlConn, CancellationToken CancellationToken);
}