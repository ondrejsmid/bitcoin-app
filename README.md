# Bitcoin Course App (Blazor + API)

A Blazor WebAssembly frontend displaying Bitcoin price data from CoinDesk, converted to CZK using CNB exchange rates via a local ASP.NET Core Web API backend.

## Projects

- **BlazorTableApp** — Blazor WebAssembly client (port 5001/http 5000)
- **BlazorTableApi** — ASP.NET Core Web API (port 5000 for HTTP)

## How to run

### Option 1: Run both from solution

```bash
cd /home/ondrej/bitcoin-app
dotnet run --project blazor-table-api &
sleep 2
dotnet run --project blazor-table-app
```

Or in separate terminals:

**Terminal 1 — API:**
```bash
cd /home/ondrej/bitcoin-app/blazor-table-api
dotnet run --urls "http://localhost:5000"
```

**Terminal 2 — Client (in another terminal):**
```bash
cd /home/ondrej/bitcoin-app/blazor-table-app
dotnet run --urls "http://localhost:5001"
```

Then open the browser to:
- **Blazor UI**: http://localhost:5001
- **API Swagger**: http://localhost:5000/swagger

### Option 2: Use dotnet watch

```bash
cd /home/ondrej/bitcoin-app/blazor-table-api
dotnet watch run --urls "http://localhost:5000"
# In another terminal
cd /home/ondrej/bitcoin-app/blazor-table-app
dotnet watch run --urls "http://localhost:5001"
```

## Architecture

- **API** (`BlazorTableApi`):
  - `CnbExchangeRateService`: Fetches EUR→CZK rate from Czech National Bank (CNB) API
  - `ExchangeRateController.GetEurToCzk()`: Returns exchange rate as JSON

- **Client** (`BlazorTableApp`):
  - `DataService.FetchFromCoinDeskAsync()`: Fetches BTC-EUR from CoinDesk, multiplies predefined keys (PRICE, LAST_PROCESSED_TRADE_PRICE, BEST_BID, BEST_ASK) by the EUR→CZK rate retrieved from local API
  - `Pages/Index.razor`: Polls every 5 seconds and displays the Bitcoin Course table

## Features

- ✅ Polls CoinDesk API every 5 seconds (no hard page refresh)
- ✅ Fetches EUR→CZK rate from CNB via local API
- ✅ Multiplies predefined price fields by exchange rate (CZK conversion)
- ✅ Preserves original JSON field order
- ✅ Shows all BTC-EUR fields from CoinDesk response

## Debugging

- Check browser DevTools Console for client-side logs
- Check terminal output for API logs (Kestrel/middleware)
- Add `Console.WriteLine()` to log trace paths
