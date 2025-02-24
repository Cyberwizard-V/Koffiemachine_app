using CelestialDrip.Data;
using CelestialDrip.Interfaces;
using CelestialDrip.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CoffeeMachineContext>(options =>
    options.UseInMemoryDatabase("CoffeeMachineDB"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//voeg logger toe
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/OVERKILLMYLOGS-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();

var app = builder.Build();

//seed inmem db
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CoffeeMachineContext>();
    context.Database.EnsureCreated(); // ensure of de database word gemaakt
}

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
