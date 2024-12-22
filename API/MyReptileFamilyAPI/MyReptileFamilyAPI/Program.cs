using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Handlers;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyLibrary.Builder;

var builder = WebBuilder.Create();
builder.WithSettings<DbSettings>();

var app = builder.BuildAndValidate();

app.UseHttpsRedirection();

app.MapPost("/register", async ([FromBody] RegisterOwner User, CancellationToken CancellationToken) => await app.Services.GetRequiredService<IRegister>().RegisterUserAsync(User, CancellationToken));
// Make the Username switchable with the email
app.MapPost("/login", async ([FromBody]Owner User, CancellationToken CancellationToken) => await app.Services.GetRequiredService<ILogIn>().UserLogIn(User, CancellationToken));
app.MapGet("/sale", async (int pageNo, int count, string reptileType, CancellationToken _p_CancellationToken) => "HOME");

app.Run();
