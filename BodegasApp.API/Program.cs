using BodegasApp.API.Services;
using BodegasApp.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Usamos el OpenAPI nativo de .NET
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Esto habilita la interfaz visual moderna de OpenAPI en .NET
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();