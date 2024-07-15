using AndBank.Api.Configuration;
using AndBank.Data.Context;
using AndBank.Process.Application.AutoMapper;
using ApiFuncional.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiConfig()
    .AddSwaggerConfig()
    .AddDbContextConfig();

builder.Services.RegisterServices();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
