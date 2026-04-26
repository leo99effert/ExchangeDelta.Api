# ExchangeDelta.Api

A REST API that calculates currency exchange rate deltas using Riksbanken's API.

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 or later

## How to run locally

1. Clone the repository
2. Open `ExchangeDelta.sln` in Visual Studio
3. Press F5 to run the project

The API will start at `https://localhost:[port]/currencydelta`

## Endpoint

POST `/currencydelta`

### Request

\```json
{
"baseline": "GBP",
"currencies": ["USD"],
"fromDate": "2025-01-01",
"toDate": "2025-01-10"
}
\```

### Response

\```json
[
{
"currency": "USD",
"delta": -0.011
}
]
\```

### Error response

\```json
{
"errorCode": "dateproblem",
"errorDetails": "To date is smaller than or equal to from date"
}
\```

## Running tests

Open Test Explorer in Visual Studio via **Test → Test Explorer** and click **Run All**.

## Known Limitations

The application uses Riksbanken's public API to fetch exchange rates. Riksbanken limits 
requests to 5 calls per minute and 1000 calls per day. If you receive an unexpected 
currency error, wait a few seconds and try again. An API key can be registered at 
https://developer.api.riksbank.se/ for higher limits.
