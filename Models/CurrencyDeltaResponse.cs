namespace ExchangeDelta.Api.Models
{
    public class CurrencyDeltaResponse
    {
        public string Currency { get; set; } = string.Empty;
        public double Delta { get; set; }
    }
}
