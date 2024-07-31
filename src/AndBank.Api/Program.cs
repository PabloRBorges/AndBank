using AndBank.Api.Configuration;
using AndBank.Data.Context;
using AndBank.Process.Application.AutoMapper;
using ApiFuncional.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiConfig()
    .AddCorsConfig()
    .AddSwaggerConfig()
    .AddDbContextConfig();

builder.Services.RegisterServices();

var app = builder.Build();

if(app.Environment.IsDevelopment())
    app.UseCors("Development");
else
    app.UseCors("Production");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
