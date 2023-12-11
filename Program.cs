// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;

IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();


if (args.Length != 3)
{
    Console.WriteLine("Wrong number of arguments. Should be quote-alert [TICKER] [min-price] [max-price] ");
}
else
{
    string ticker = args[0];
    decimal minprice = Convert.ToDecimal(args[1]);
    decimal maxprice = Convert.ToDecimal(args[2]);





}

Console.WriteLine("Hello, World!");
Console.WriteLine(Configuration.GetValue<string>("emaildestino"));