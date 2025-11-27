# Bitcoin App

A web application for viewing live Bitcoin price data and managing snapshots with notes.

## Stručný popis projektu

Aplikace umožňuje sledovat aktuální cenu Bitcoinu v EUR a CZK s automatickou aktualizací každých 5 sekund. Uživatel může ukládat snapshoty s vlastními poznámkami, prohlížet je, filtrovat, řadit a upravovat. Aplikace je postavena na ASP.NET Core Web API pro backend, ASP.NET Web Forms pro frontend a SQL Server pro ukládání dat.

### Jak smazat snapshot

1. Přejít na stránku **Saved Data** (`/Stored.aspx`)
2. V levém panelu najít snapshot, který chcete smazat
3. Kliknout na tlačítko **View** vedle snapshotu pro zobrazení detailu
4. V detailu snapshotu (pravý panel) kliknout na červené tlačítko **Delete Snapshot** nahoře
5. Potvrdit smazání v dialogu
6. Snapshot bude trvale odstraněn z databáze a seznam se aktualizuje


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
  - Delete snapshots
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

## API Testing

The `api-examples` folder contains PowerShell scripts for testing all API endpoints:

- **01-get-btc-data.ps1** - Fetch current live Bitcoin price data
- **02-save-snapshot.ps1** - Save current data as a snapshot with a note
- **03-get-all-snapshots.ps1** - List all saved snapshots
- **04-get-snapshot-by-id.ps1** - Retrieve a specific snapshot by ID
- **05-update-snapshot-note.ps1** - Update a snapshot's note
- **06-delete-snapshot.ps1** - Delete a snapshot permanently
- **07-get-last-snapshot.ps1** - Get the most recently saved snapshot

See `api-examples/README.md` for detailed documentation on each script and usage instructions.
