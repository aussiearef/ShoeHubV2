using Prometheus;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var randomGenerator = new Random();

var countryCodes = new[] { "AU", "US", "IN" };
var paymentMethods = new[] { "Card", "Cash", "Paypal" };
var shoeTypes = new[] { "Loafers", "Boots", "HighHeels" };

app.UseHttpsRedirection();
app.UseHttpMetrics();
app.MapMetrics().DisableRateLimiting();

app.Use(async (context, next) =>
{
    foreach (var shoeType in shoeTypes)
    {
        var soldValue = randomGenerator.Next(10, 100);
        var salesMetric = Metrics.CreateCounter($"shoehub_sales_{shoeType}", $"Sales of {shoeType}.");
        salesMetric.Inc(soldValue);
    }

    foreach (var countryCode in countryCodes)
    {
        foreach (var paymentMethod in paymentMethods)
        {
            var paymentValue = randomGenerator.Next(1000);
            var paymentMetric = Metrics.CreateGauge($"shoehub_{countryCode}_payments_{paymentMethod}", $"Payment in {countryCode} using {paymentMethod}.");
            paymentMetric.Set(paymentValue);
        }
    }

    await next();
});

app.Run();