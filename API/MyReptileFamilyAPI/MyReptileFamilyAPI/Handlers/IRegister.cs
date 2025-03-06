using MyReptileFamilyAPI.Models;

namespace MyReptileFamilyAPI.Handlers;

public interface IRegister
{
    Task<IResult> RegisterUserAsync(RegisterOwner Owner, CancellationToken CancellationToken);
    Task<IResult> AuthUserAsync(string Username, string Token, CancellationToken CancellationToken);
}