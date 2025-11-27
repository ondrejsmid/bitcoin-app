# Bitcoin App

A web application for viewing live Bitcoin price data and managing snapshots with notes.

## Quick Start

### 1. Setup Database
Ensure SQL Server is running on your machine (e.g., SQL Server Express at `localhost\SQLEXPRESS`).

Connect to SQL Server using SQL Server Management Studio (SSMS) or your preferred SQL client, and run the `init.sql` script:
- This creates the `ondrej-bitcoinapp` database
- Creates the `Snapshots` and `SnapshotRows` tables

### 2. Start Backend API
```bash
cd BitcoinCourse/BitcoinCourseAPI
dotnet run
```

The API will start on http://localhost:5041

### 3. Start Frontend
```bash
cd BitcoinCourse/BitcoinCourseUI
dotnet run
```

The frontend will start on a port specified in the output (typically http://localhost:5000 or similar).

### 4. Access from Web Browser
Open your browser and navigate to the frontend URL (e.g., http://localhost:5000)

## Pages

- **Home** (`/Default.aspx`) — Live Bitcoin price data from CoinDesk with EUR→CZK conversion
  - View real-time BTC-EUR prices
  - Automatic 5-second refresh
  - Save snapshots with custom notes
  
- **Stored Data** (`/Stored.aspx`) — View and manage saved snapshots
  - List all saved snapshots with filtering and sorting
  - View snapshot details
  - Edit snapshot notes
  - Filter and sort snapshot data

## Features

- ✅ Live Bitcoin prices from CoinDesk API
- ✅ EUR to CZK exchange rate conversion via CNB
- ✅ 5-second auto-refresh on home page
- ✅ Save snapshots with custom notes
- ✅ View all saved snapshots
- ✅ Edit snapshot notes
- ✅ Column sorting and filtering on all tables
- ✅ SQL Server database integration

## Database Setup

1. Ensure SQL Server is running (via Docker or local installation)
2. Connect to the database using your preferred SQL client
3. Run the `init.sql` script to create:
   - `ondrej-bitcoinapp` database
   - `Snapshots` table
   - `SnapshotRows` table

## Architecture

- **Backend**: ASP.NET Core Web API
- **Frontend**: ASP.NET Web Forms
- **Database**: SQL Server
- **External APIs**: CoinDesk (Bitcoin prices), CNB (Exchange rates)
