namespace ExchangeDelta.Api.Models
{
    public class CurrencyDeltaRequest
    {
        public string? Baseline { get; set; }
        public List<string>? Currencies { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
