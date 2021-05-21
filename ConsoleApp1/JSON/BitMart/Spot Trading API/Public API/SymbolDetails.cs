
public class SymbolDetails
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public SymbolDetailsData data { get; set; }
}

public class SymbolDetailsData
{
    public SymbolDetailDataList[] symbols { get; set; }
}

public class SymbolDetailDataList
{
    public string symbol { get; set; }
    public int symbol_id { get; set; }
    public string base_currency { get; set; }
    public string quote_currency { get; set; }
    public string quote_increment { get; set; }
    public string base_min_size { get; set; }
    public string base_max_size { get; set; }
    public int price_min_precision { get; set; }
    public int price_max_precision { get; set; }
    public string expiration { get; set; }
    public string min_buy_amount { get; set; }
    public string min_sell_amount { get; set; }
}
