using ExchangeDelta.Api.Models;
using ExchangeDelta.Api.Validators;

namespace ExchangeDelta.Api.Services
{
    public class CurrencyDeltaService
    {
        private readonly RiksbankService _riksbankService;
        private readonly RequestValidator _validator;

        public CurrencyDeltaService(RiksbankService riksbankService, RequestValidator validator)
        {
            _riksbankService = riksbankService;
            _validator = validator;
        }

        public async Task<(List<CurrencyDeltaResponse>? result, ErrorResponse? error)> GetDeltasAsync(CurrencyDeltaRequest request)
        {
            var validationError = _validator.Validate(request);
            if (validationError != null)
                return (null, validationError);

            var results = new List<CurrencyDeltaResponse>();

            foreach (var currency in request.Currencies!)
            {
                bool invertResult = false;
                string fetchCurrency = currency;

                if (currency.Equals("SEK", StringComparison.OrdinalIgnoreCase))
                {
                    fetchCurrency = request.Baseline!;
                    invertResult = true;
                }

                var observations = await _riksbankService.GetRatesAsync(request.Baseline!, currency, request.FromDate!, request.ToDate!);

                if (observations == null || observations.Count == 0)
                    return (null, new ErrorResponse
                    {
                        ErrorCode = "currencyproblem",
                        ErrorDetails = $"Currency {currency} does not exist or has no data for the given dates"
                    });

                var fromValue = _riksbankService.GetClosestValue(observations, request.FromDate!);
                var toValue = _riksbankService.GetClosestValue(observations, request.ToDate!);

                if (invertResult)
                {
                    fromValue = 1 / fromValue;
                    toValue = 1 / toValue;
                }

                var delta = Math.Round(toValue - fromValue, 3);

                results.Add(new CurrencyDeltaResponse
                {
                    Currency = currency,
                    Delta = delta
                });
            }

            return (results, null);
        }
    }
}