using MyReptileFamilyAPI.Models;

namespace MyReptileFamilyAPI.Handlers;

public interface IRegister
{
    Task<IResult> RegisterUserAsync(RegisterOwner Owner, CancellationToken CancellationToken);
}