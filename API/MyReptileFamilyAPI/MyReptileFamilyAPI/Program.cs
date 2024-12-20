using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Handlers;
using MyReptileFamilyLibrary.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton((IServiceProvider Provider) => Provider.GetService<IOptions<DbSettings>>().Value);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ILogIn logIn = new LogIn();
IRegister register = new Register();

app.UseHttpsRedirection();

app.MapPost("/register", async ([FromBody] User user, CancellationToken _p_CancellationToken) => await register.RegisterUser(user));
// Make the Username switchable with the email
app.MapPost("/login", async ([FromBody]User user, CancellationToken _p_CancellationToken) => await logIn.UserLogIn(user));
app.MapGet("/sale", async (int pageNo, int count, string reptileType, CancellationToken _p_CancellationToken) => "HOME");

app.Run();
