using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Configurations;
using ISWBlacklist.Extentions;
using ISWBlacklist.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddLoggingConfiguration(config);
builder.Services.AuthenticationConfiguration(config);
//builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ISBWBlacklist v1"));
}
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await Seeder.SeedRolesAndAdmins(serviceProvider);
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

