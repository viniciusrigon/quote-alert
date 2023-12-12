using System;
using System.Text.Json.Serialization;

namespace QuoteAlert;

public class TickerResult
{
    [JsonPropertyName("results")]
    public Ticker[] Quotes { get; set; }
}