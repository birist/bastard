
public class OrderResponse
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public OrderResponseData data { get; set; }
}

public class OrderResponseData
{
    public long order_id { get; set; }
}
