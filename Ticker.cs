using System;
using Newtonsoft.Json;

namespace QuoteAlert;

public class Ticker
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("shortName")]
    public string ShortName { get; set; }

    [JsonProperty("longName")]
    public string LongName { get; set; }

    [JsonProperty("regularMarketPrice")]
    public decimal RegularMarketPrice { get; set; }

    [JsonProperty("regularMarketDayHigh")]
    public decimal RegularMarketDayHigh { get; set; }

    [JsonProperty("regularMarketDayLow")]
    public decimal RegularMarketDayLow { get; set; }

    [JsonProperty("regularMarketDayRange")]
    public string RegularMarketDayRange { get; set; }

    [JsonProperty("regularMarketChange")]
    public decimal RegularMarketChange { get; set; }

    [JsonProperty("regularMarketChangePercent")]
    public decimal RegularMarketChangePercent { get; set; }

    [JsonProperty("regularMarketTime")]
    public DateTime RegularMarketTime { get; set; }


}