using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");
});

var app = builder.Build();

var randomGenerator = new Random();
var countryCodes = new[] { "AU", "US", "IN" };
var paymentMethods = new[] { "Card", "Cash", "Paypal" };
var shoeTypes = new[] { "Loafers", "Boots", "HighHeels" };

// Resolve ILogger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.UseHttpsRedirection();
app.UseHttpMetrics();
app.MapMetrics().DisableRateLimiting();

app.Use(async (context, next) =>
{
    logger.LogInformation("Processing request for path: {Path}", context.Request.Path);

    foreach (var shoeType in shoeTypes)
    {
        var soldValue = randomGenerator.Next(10, 100);
        var salesMetric = Metrics.CreateCounter($"shoehub_sales", "", "ShoeType")
            .WithLabels(shoeType);
        salesMetric.Inc(soldValue);
        logger.LogInformation("Updated sales metric for {ShoeType} with value {SoldValue}", shoeType, soldValue);
    }

    foreach (var countryCode in countryCodes)
    {
        foreach (var paymentMethod in paymentMethods)
        {
            var paymentValue = randomGenerator.Next(1000);
            var paymentMetric = Metrics.CreateGauge($"shoehub_payments", "", "CountryCode", "PaymentMethod")
                .WithLabels(countryCode, paymentMethod);
            paymentMetric.Set(paymentValue);
            logger.LogInformation("Updated payment metric for {CountryCode}, {PaymentMethod} with value {PaymentValue}",
                countryCode, paymentMethod, paymentValue);
        }
    }

    await next();
});

app.Run();