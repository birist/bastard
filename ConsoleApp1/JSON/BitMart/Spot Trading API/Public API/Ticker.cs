
public class Ticker
{
    public string message { get; set; }
    public int code { get; set; }
    public string trace { get; set; }
    public TickerData data { get; set; }
}

public class TickerData
{
    public TickerList[] tickers { get; set; }
}

public class TickerList
{
    public string symbol { get; set; }
    public string last_price { get; set; }
    public string quote_volume_24h { get; set; }
    public string base_volume_24h { get; set; }
    public string high_24h { get; set; }
    public string low_24h { get; set; }
    public string open_24h { get; set; }
    public string close_24h { get; set; }
    public string best_ask { get; set; }
    public string best_ask_size { get; set; }
    public string best_bid { get; set; }
    public string best_bid_size { get; set; }
    public string fluctuation { get; set; }
    public string url { get; set; }
}
