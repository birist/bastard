
public class Kline
{
    public string message { get; set; }
    public int code { get; set; }
    public string trace { get; set; }
    public KlineData data { get; set; }
}

public class KlineData
{
    public Klines[] klines { get; set; }
}

public class Klines
{
    public int timestamp { get; set; }
    public string open { get; set; }
    public string high { get; set; }
    public string low { get; set; }
    public string close { get; set; }
    public string last_price { get; set; }
    public string volume { get; set; }
}
