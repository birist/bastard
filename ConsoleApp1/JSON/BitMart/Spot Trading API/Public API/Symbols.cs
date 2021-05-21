
public class Symbols
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public SymbolsData data { get; set; }
}

public class SymbolsData
{
    public string[] symbols { get; set; }
}
