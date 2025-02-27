using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Handlers;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.Record;
using MyReptileFamilyLibrary.Builder;

WebBuilder builder = WebBuilder.Create();
builder.WithSettings<DbSettings>();
EmailSettings emailSettings = builder.WithSettings<EmailSettings>();
APISettings apiSettings = builder.WithSettings<APISettings>();

builder.SetPort(apiSettings.Port, apiSettings.PathToCert, apiSettings.CertPassword, apiSettings.URL);

builder.WithEmailService(emailSettings);

builder.WithCors("AllowSpecificOrigins", $"{apiSettings.URL}:{apiSettings.Port}");

WebApplication app = builder.BuildAndValidate();

app.UseCors("AllowSpecificOrigins");

#if DEBUG
app.UseHttpsRedirection();
#endif

RouteGroupBuilder routeGroup = app.MapGroup("/api");

routeGroup.MapPost("/register",
    async ([FromBody] RegisterOwner User, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<IRegister>().RegisterUserAsync(User, CancellationToken));

routeGroup.MapPost("/auth",
    async (string Username, string Token, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<IRegister>().AuthUserAsync(Username, Token, CancellationToken));

// Make the Username switchable with the email
routeGroup.MapPost("/login",
    async ([FromBody] Owner User, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<ILogIn>().UserLogInAsync(User, CancellationToken));

routeGroup.MapGet("/sale", (int PageNo, int Count, string ReptileType, CancellationToken CancellationToken) => "HOME");


RouteGroupBuilder messagesGroup = app.MapGroup("/messages");

messagesGroup.MapPost("/send",
    async ([FromBody] MessageDTO Message, CancellationToken CancellationToken) =>
    await app.Services.GetRequiredService<IMessages>().SendAsync(Message, CancellationToken));

messagesGroup.MapGet("/{receiverId:long}/{senderId:long}/{count:int}",
    async(long ReceiverId, long SenderId, int Count, CancellationToken CancellationToken) => 
    await app.Services.GetRequiredService<IMessages>().GetMessages(ReceiverId, SenderId, Count, CancellationToken));

await app.RunAsync();