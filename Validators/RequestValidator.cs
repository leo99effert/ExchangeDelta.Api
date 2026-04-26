
using ExchangeDelta.Api.Models;

namespace ExchangeDelta.Api.Validators
{
    public class RequestValidator
    {
        public ErrorResponse? Validate(CurrencyDeltaRequest request)
        {
            if (request.Baseline == null || request.Currencies == null || request.FromDate == null || request.ToDate == null)
                return new ErrorResponse { ErrorCode = "missingfields", ErrorDetails = "One or more required fields are missing" };

            if (!DateOnly.TryParse(request.FromDate, out DateOnly fromDate) || !DateOnly.TryParse(request.ToDate, out DateOnly toDate))
                return new ErrorResponse { ErrorCode = "dateproblem", ErrorDetails = "One or more dates are in wrong format" };

            if (fromDate >= toDate)
                return new ErrorResponse { ErrorCode = "dateproblem", ErrorDetails = "To date is smaller than or equal to from date" };

            if (fromDate.Year < 2023)
                return new ErrorResponse { ErrorCode = "dateproblem", ErrorDetails = "From date must not be earlier than 2023" };

            if (request.Currencies.Count == 0)
                return new ErrorResponse { ErrorCode = "currencyproblem", ErrorDetails = "Currencies list must not be empty" };

            if (request.Currencies.Count != request.Currencies.Distinct().Count())
                return new ErrorResponse { ErrorCode = "currencyproblem", ErrorDetails = "Currencies must all be unique" };

            if (request.Currencies.Contains(request.Baseline, StringComparer.OrdinalIgnoreCase))
                return new ErrorResponse { ErrorCode = "currencyproblem", ErrorDetails = "Currencies must not contain the baseline currency" };

            return null;
        }
    }
}