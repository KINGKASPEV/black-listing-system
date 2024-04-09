using ISWBlacklist.Application.Services.Implementations;
using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Configurations;
using ISWBlacklist.Domain.Entities;
using ISWBlacklist.Extentions;
using ISWBlacklist.Infrastructure.Context;
using ISWBlacklist.Infrastructure.Repositories.Implementations;
using ISWBlacklist.Infrastructure.Repositories.Interfaces;
using ISWBlacklist.Mapper;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthenticationServices(config);


builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddLoggingConfiguration(config);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
