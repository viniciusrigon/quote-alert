using System;
using Newtonsoft.Json;

namespace QuoteAlert;

public class TickerResult
{
    [JsonProperty("results")]
    public Ticker[] Quotes { get; set; }
}