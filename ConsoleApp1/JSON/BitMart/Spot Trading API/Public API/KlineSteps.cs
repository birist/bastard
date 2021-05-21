
public class KlineSteps
{
    public int code { get; set; }
    public string trace { get; set; }
    public string message { get; set; }
    public KlineStepsData data { get; set; }
}

public class KlineStepsData
{
    public int[] steps { get; set; }
}
