using BitcoinCourseAPI.Services;
using BitcoinCourseAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register BtcDataService with an HttpClient
builder.Services.AddHttpClient<IBtcDataService, BtcDataService>();
builder.Services.AddTransient<ICnbConversionService, CnbConversionService>();
builder.Services.AddTransient<ISnapshotsService, SnapshotsService>();

// Register EF DbContext

builder.Services.AddDbContext<BitcoinDbContext>(opts => opts.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ondrej-bitcoinapp;Trusted_Connection=True;TrustServerCertificate=True;"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
