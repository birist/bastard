
public class Currency
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public Currency data { get; set; }
}

public class CurrencyData
{
    public Currencies[] currencies { get; set; }
}

public class Currencies
{
    public string id { get; set; }
    public string name { get; set; }
    public bool withdraw_enabled { get; set; }
    public bool deposit_enabled { get; set; }
}
