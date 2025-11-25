namespace BlazorTableApi.Models;

public class ExchangeRateDto
{
    public decimal Rate { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public string TargetCode { get; set; } = "CZK";
    public DateTime FetchedAt { get; set; }
}
