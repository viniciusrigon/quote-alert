// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using QuoteAlert;
using RestSharp;


var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();
var cfg = config.Get<AppConfiguration>();


if (args.Length != 3)
{
    Console.WriteLine("Wrong number of arguments. Should be quote-alert [TICKER] [min-price] [max-price] ");
}
else
{
    string ticker = args[0];
    decimal minprice = Convert.ToDecimal(args[1]);
    decimal maxprice = Convert.ToDecimal(args[2]);
    var queryQuote = $"{ticker}?range=1d&interval=1d&fundamental=false";


    var apiClient = new RestClient(cfg.ApiConfig.baseUrl);


    var request = new RestRequest(cfg.ApiConfig.endpoints.quote.Replace("{tickers}", queryQuote));
    request.AddHeader("Authorization", $"Bearer {cfg.ApiConfig.token}");

    var response = apiClient.Get(request);
    if (response.IsSuccessStatusCode)
    {
        var data = JsonSerializer.Deserialize<TickerResult>(response.Content!)!;

        Console.WriteLine(data.Quotes.FirstOrDefault().RegularMarketPrice);


    }
}
