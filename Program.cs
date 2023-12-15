// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using QuoteAlert;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();
var cfg = config.Get<AppConfiguration>();

if (args.Length != 3)
{
    Console.WriteLine("Wrong number of arguments. Should be quote-alert '[TICKER]' '[sell-price]' '[buy-price]'");
}
else
{
    string ticker = args[0];
    decimal sellPrice = Convert.ToDecimal(args[1].Replace(",", "."));
    decimal buyPrice = Convert.ToDecimal(args[2].Replace(",", "."));

    var queryQuote = $"{ticker}?range=1d&interval=1d&fundamental=false";

    var apiClient = new RestClient(cfg.ApiConfig.baseUrl);
    var request = new RestRequest(cfg.ApiConfig.endpoints.quote.Replace("{tickers}", queryQuote));
    request.AddHeader("Authorization", $"Bearer {cfg.ApiConfig.token}");

    Console.WriteLine("Pressione ESC para sair");
    do
    {
        while (!Console.KeyAvailable)
        {
            var response = apiClient.Get(request);
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<TickerResult>(response.Content?.ToString());

                var quote = data?.Quotes?.FirstOrDefault();
                if (quote != null)
                {
                    if (quote.RegularMarketPrice >= sellPrice)
                    {
                        await SendEmailSellQuote(quote, sellPrice, cfg.emaildestino);
                    }

                    if (quote.RegularMarketPrice <= buyPrice)
                    {
                        await SendEmailBuyQuote(quote, buyPrice, cfg.emaildestino);
                    }
                }

                Thread.Sleep(2000);  // evitar muitas requisições por segundo
            }
            else
            {
                Console.WriteLine($"Ocorreu o erro {response.ErrorMessage} ao buscar a cotação do ticker {ticker}");
                break;
            }
        }

    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

    Console.WriteLine("QuoteAlert - fechado");

}

async Task SendEmailBuyQuote(Ticker quote, decimal buyPrice, string emaildestino)
{
    MailMessage mail = new MailMessage()
    {
        From = new MailAddress(cfg.SmtpConfig.user, "Vinicius Rigon")
    };

    mail.To.Add(new MailAddress(emaildestino));

    mail.Subject = "Aviso de compra de ação";
    mail.Body = $"O preço da ação {quote.ShortName} caiu abaixo de {buyPrice.ToString("C2")} {Environment.NewLine} Cotação atual: {quote.RegularMarketPrice.ToString("C2")}";
    mail.IsBodyHtml = true;

    using (SmtpClient smtp = new SmtpClient(cfg.SmtpConfig.server, 587))
    {
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(cfg.SmtpConfig.user, cfg.SmtpConfig.password);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);
        Console.WriteLine("E-mail de compra de ação enviado.");
    }
}

async Task SendEmailSellQuote(Ticker quote, decimal sellPrice, string emaildestino)
{
    MailMessage mail = new MailMessage()
    {
        From = new MailAddress(cfg.SmtpConfig.user, "Vinicius Rigon")
    };

    mail.To.Add(new MailAddress(emaildestino));

    mail.Subject = "Aviso de venda de ação";
    mail.Body = $"O preço da ação {quote.ShortName} subiu acima de {sellPrice.ToString("C2")} {Environment.NewLine} Cotação atual: {quote.RegularMarketPrice.ToString("C2")}";
    mail.IsBodyHtml = true;

    using (SmtpClient smtp = new SmtpClient(cfg.SmtpConfig.server, 587))
    {
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(cfg.SmtpConfig.user, cfg.SmtpConfig.password);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);
        Console.WriteLine("E-mail de venda de ação enviado.");
    }
}