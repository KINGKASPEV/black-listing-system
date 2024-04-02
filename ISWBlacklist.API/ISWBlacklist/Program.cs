// using ISWBlacklist.API.ISWBlacklist.Repositories;
using ISWBlacklist.Extentions;
using ISWBlacklist.Mapper;
using ISWBlacklist.API.ISWBlacklist.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<IUserRepository, SQLUserRepository>();


builder.Services.AddAutoMapper(typeof(MapperProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
