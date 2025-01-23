using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Handlers;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyLibrary.Builder;

WebBuilder builder = WebBuilder.Create();
builder.WithSettings<DbSettings>();
EmailSettings emailSettings = builder.WithSettings<EmailSettings>();

builder.WithEmailService(emailSettings);

WebApplication app = builder.BuildAndValidate();

#if DEBUG
app.UseHttpsRedirection();
#endif

RouteGroupBuilder _routeGroup = app.MapGroup("/api");

_routeGroup.MapPost("/register",
    async ([FromBody] RegisterOwner User, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<IRegister>().RegisterUserAsync(User, CancellationToken));

_routeGroup.MapPost("/auth",
    async (string Username, string Token, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<IRegister>().AuthUserAsync(Username, Token, CancellationToken));

// Make the Username switchable with the email
_routeGroup.MapPost("/login",
    async ([FromBody] Owner User, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<ILogIn>().UserLogIn(User, CancellationToken));

_routeGroup.MapGet("/sale", (int PageNo, int Count, string ReptileType, CancellationToken CancellationToken) => "HOME");

await app.RunAsync();