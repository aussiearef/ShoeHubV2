using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


var randomGenerator = new Random(DateTime.Now.Millisecond);

var countryCodes = new[] { "AU", "US", "IN" };
var paymentMethods = new[] { "Card", "Cash", "Paypal" };
var shoeTypes = new[] { "Loafers", "Boots", "HighHeels" };

Counter salesMetric;
Gauge paymentMetric;

#region Sales Metrics

foreach (var shoeType in shoeTypes)
{
    var soldValue = randomGenerator.Next(10, 100);
    salesMetric = Metrics.CreateCounter($"shoehub_sales_{shoeType}", $"Sales of {shoeType}.");
    salesMetric.Inc(soldValue);
}

foreach (var countryCode in countryCodes)
{
    var paymentMethod = paymentMethods[randomGenerator.Next(paymentMethods.Length)];
    var paymentValue = randomGenerator.Next(1000);

    paymentMetric = Metrics.CreateGauge($"shoehub_{countryCode}_payments_{paymentMethod}", $"Payment in {countryCode}");
    paymentMetric.Set(paymentValue);
}

#endregion


app.UseHttpsRedirection();
app.UseHttpMetrics();
app.MapMetrics(pattern:"/").DisableRateLimiting();

app.Run();