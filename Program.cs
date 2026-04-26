using ExchangeDelta.Api.Services;
using ExchangeDelta.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<RiksbankService>();
builder.Services.AddScoped<RequestValidator>();
builder.Services.AddScoped<CurrencyDeltaService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();