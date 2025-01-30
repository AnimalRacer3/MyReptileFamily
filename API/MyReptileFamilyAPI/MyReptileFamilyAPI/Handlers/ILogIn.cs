using MyReptileFamilyAPI.Models;

namespace MyReptileFamilyAPI.Handlers;

public interface ILogIn
{
    Task<IResult> UserLogInAsync(Owner User, CancellationToken Cancellation);
}