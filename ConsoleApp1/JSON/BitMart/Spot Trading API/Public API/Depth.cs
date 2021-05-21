
public class Depth
{
    public string message { get; set; }
    public int code { get; set; }
    public string trace { get; set; }
    public DepthData data { get; set; }
}

public class DepthData
{
    public long timestamp { get; set; }
    public Buy[] buys { get; set; }
    public Sell[] sells { get; set; }
}

public class Buy
{
    public string amount { get; set; }
    public string total { get; set; }
    public string price { get; set; }
    public string count { get; set; }
}

public class Sell
{
    public string amount { get; set; }
    public string total { get; set; }
    public string price { get; set; }
    public string count { get; set; }
}
