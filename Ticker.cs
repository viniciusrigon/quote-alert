using System;
using System.Text.Json.Serialization;

namespace QuoteAlert;

public class Ticker
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("longName")]
    public string LongName { get; set; }

    [JsonPropertyName("regularMarketPrice")]
    public float RegularMarketPrice { get; set; }

    [JsonPropertyName("regularMarketDayHigh")]
    public int RegularMarketDayHigh { get; set; }

    [JsonPropertyName("regularMarketDayLow")]
    public int RegularMarketDayLow { get; set; }

    [JsonPropertyName("regularMarketDayRange")]
    public string RegularMarketDayRange { get; set; }

    [JsonPropertyName("regularMarketChange")]
    public float RegularMarketChange { get; set; }

    [JsonPropertyName("regularMarketChangePercent")]
    public float RegularMarketChangePercent { get; set; }

    [JsonPropertyName("regularMarketTime")]
    public DateTime RegularMarketTime { get; set; }


}