
public class OrderDetailResponse
{
    public string message { get; set; }
    public int code { get; set; }
    public string trace { get; set; }
    public OrderDetailResponseData data { get; set; }
}

public class OrderDetailResponseData
{
    public long order_id { get; set; }
    public string symbol { get; set; }
    public long create_time { get; set; }
    public string side { get; set; }
    public string type { get; set; }
    public string price { get; set; }
    public string price_avg { get; set; }
    public string size { get; set; }
    public string notional { get; set; }
    public string filled_notional { get; set; }
    public string filled_size { get; set; }
    public string status { get; set; }
}
