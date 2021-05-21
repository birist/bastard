
public class Trades
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public TradesData data { get; set; }
}

public class TradesData
{
    public TradesDataList[] trades { get; set; }
}

public class TradesDataList
{
    public string amount { get; set; }
    public long order_time { get; set; }
    public string price { get; set; }
    public string count { get; set; }
    public string type { get; set; }
}
