public class AppConfiguration
{

    public string emaildestino { get; set; }
    public SmtpConfig SmtpConfig { get; set; }

    public ApiConfig ApiConfig { get; set; }

}

public class SmtpConfig
{
    public string server { get; set; }
    public string user { get; set; }
    public string password { get; set; }
    public string from { get; set; }
}

public class ApiConfig
{
    public string token { get; set; }
    public string baseUrl { get; set; }
    public EndpointsConfig endpoints { get; set; }
}

public class EndpointsConfig
{
    public string quote { get; set; }
}