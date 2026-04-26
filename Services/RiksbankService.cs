using ExchangeDelta.Api.Models;
using System.Text.Json;

namespace ExchangeDelta.Api.Services
{
    public class RiksbankService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.riksbank.se/swea/v1";

        public RiksbankService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RiksbankObservation>> GetRatesAsync(string baseline, string currency, string fromDate, string toDate)
        {
            string url;

            if (baseline.Equals("SEK", StringComparison.OrdinalIgnoreCase))
            {
                url = $"{BaseUrl}/observations/sek{currency.ToLower()}pmi/{fromDate}/{toDate}";
            }
            else if (currency.Equals("SEK", StringComparison.OrdinalIgnoreCase))
            {
                url = $"{BaseUrl}/observations/sek{baseline.ToLower()}pmi/{fromDate}/{toDate}";
            }
            else
            {
                url = $"{BaseUrl}/CrossRates/sek{baseline.ToLower()}pmi/sek{currency.ToLower()}pmi/{fromDate}/{toDate}";
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return new List<RiksbankObservation>();

            return JsonSerializer.Deserialize<List<RiksbankObservation>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<RiksbankObservation>();
        }

        public double GetClosestValue(List<RiksbankObservation> observations, string targetDate)
        {
            var target = DateOnly.Parse(targetDate);

            return observations
                .OrderBy(o => Math.Abs((DateOnly.Parse(o.Date).ToDateTime(TimeOnly.MinValue) - target.ToDateTime(TimeOnly.MinValue)).Days))
                .First()
                .Value;
        }
    }
}