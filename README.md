# Blazor Table App

## Quick Start

### Terminal 1: Start Database
```bash
docker-compose up -d
```

### Terminal 2: Start API
```bash
cd blazor-table-api
dotnet run --urls "http://localhost:5000"
```

### Terminal 3: Start Client
```bash
cd blazor-table-app
dotnet run --urls "http://localhost:5001"
```

Then open http://localhost:5001 in your browser.

## Pages

- **Bitcoin Course** (`/`) — Live Bitcoin price data from CoinDesk with EUR→CZK conversion
- **Stored Data** (`/stored`) — Database rows from SQL Server (currently showing seed data)

## Features

- ✅ Live Bitcoin prices from CoinDesk API
- ✅ EUR to CZK exchange rate conversion
- ✅ 5-second auto-polling (no hard refresh)
- ✅ SQL Server database integration
- ✅ Navigation between pages

## Additional Resources

For detailed setup instructions, troubleshooting, and architecture overview, see the complete documentation in the root directory.
