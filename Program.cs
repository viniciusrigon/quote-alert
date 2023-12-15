// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using QuoteAlert;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Security.AccessControl;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();
var cfg = config.Get<AppConfiguration>();

if (args.Length != 3)
{
    Console.WriteLine("Para executar entre com o seguinte comando: quote-alert '[TICKER]' '[sell-price]' '[buy-price]'");
}
else
{
    string ticker = args[0];
    decimal sellPrice = Convert.ToDecimal(args[1], CultureInfo.GetCultureInfo("pt-BR"));
    decimal buyPrice = Convert.ToDecimal(args[2], CultureInfo.GetCultureInfo("pt-BR"));

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
                        string assunto = "Aviso de venda de ação";
                        string body = $"O preço da ação {quote.ShortName} subiu acima de {sellPrice.ToString("C2")} {Environment.NewLine} Cotação atual: {quote.RegularMarketPrice.ToString("C2")}";

                        await SendEmail(assunto, body);
                        Console.WriteLine("E-mail de venda de ação enviado.");
                    }

                    if (quote.RegularMarketPrice <= buyPrice)
                    {
                        string assunto = "Aviso de compra de ação";
                        string body = $"O preço da ação {quote.ShortName} caiu abaixo de {buyPrice.ToString("C2")} {Environment.NewLine} Cotação atual: {quote.RegularMarketPrice.ToString("C2")}";

                        await SendEmail(assunto, body);
                        Console.WriteLine("E-mail de compra de ação enviado.");
                    }
                }

                Thread.Sleep(cfg.intervaloExecucao);  // evitar muitas requisições por segundo
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

async Task SendEmail(string assunto, string mensagemEmail)
{
    MailMessage mail = new MailMessage()
    {
        From = new MailAddress(cfg.SmtpConfig.user, cfg.SmtpConfig.from)
    };

    mail.To.Add(new MailAddress(cfg.emaildestino));

    mail.Subject = assunto;
    mail.Body = mensagemEmail;
    mail.IsBodyHtml = true;

    using (SmtpClient smtp = new SmtpClient(cfg.SmtpConfig.server, cfg.SmtpConfig.port))
    {
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(cfg.SmtpConfig.user, cfg.SmtpConfig.password);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);
    }
}