using MyReptileFamilyAPI.Models;

namespace MyReptileFamilyAPI.Services;

public interface IRegisterService
{
    Task<IResult> ValidateOwnerWithDatabaseAsync(RegisterOwner Owner, CancellationToken CancellationToken);
    Task CreateValidationAndWelcomeEmailAsync(RegisterOwner Owner, CancellationToken CancellationToken);
}