using ExchangeDelta.Api.Models;
using ExchangeDelta.Api.Validators;

namespace ExchangeDelta.Tests
{
    public class RequestValidatorTests
    {
        private readonly RequestValidator _validator = new RequestValidator();

        [Fact]
        public void Validate_ValidRequest_ReturnsNull()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "USD" },
                FromDate = "2025-01-01",
                ToDate = "2025-01-10"
            };

            var result = _validator.Validate(request);

            Assert.Null(result);
        }

        [Fact]
        public void Validate_ToDateBeforeFromDate_ReturnsError()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "USD" },
                FromDate = "2025-01-10",
                ToDate = "2025-01-01"
            };

            var result = _validator.Validate(request);

            Assert.NotNull(result);
            Assert.Equal("dateproblem", result.ErrorCode);
        }

        [Fact]
        public void Validate_FromDateBefore2023_ReturnsError()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "USD" },
                FromDate = "2022-01-01",
                ToDate = "2025-01-10"
            };

            var result = _validator.Validate(request);

            Assert.NotNull(result);
            Assert.Equal("dateproblem", result.ErrorCode);
        }

        [Fact]
        public void Validate_DuplicateCurrencies_ReturnsError()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "USD", "USD" },
                FromDate = "2025-01-01",
                ToDate = "2025-01-10"
            };

            var result = _validator.Validate(request);

            Assert.NotNull(result);
            Assert.Equal("currencyproblem", result.ErrorCode);
        }

        [Fact]
        public void Validate_CurrencySameAsBaseline_ReturnsError()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "GBP" },
                FromDate = "2025-01-01",
                ToDate = "2025-01-10"
            };

            var result = _validator.Validate(request);

            Assert.NotNull(result);
            Assert.Equal("currencyproblem", result.ErrorCode);
        }

        [Fact]
        public void Validate_InvalidDateFormat_ReturnsError()
        {
            var request = new CurrencyDeltaRequest
            {
                Baseline = "GBP",
                Currencies = new List<string> { "USD" },
                FromDate = "not-a-date",
                ToDate = "2025-01-10"
            };

            var result = _validator.Validate(request);

            Assert.NotNull(result);
            Assert.Equal("dateproblem", result.ErrorCode);
        }
    }
}