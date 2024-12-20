using MyReptileFamilyAPI.Models;

namespace MyReptileFamilyAPI.Handlers;

public interface ILogIn
{
    Task<IResult> UserLogIn(Owner User, CancellationToken Cancellation);
}