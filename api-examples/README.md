# API Examples - PowerShell Scripts

This folder contains PowerShell scripts to test and interact with the Bitcoin App API endpoints.

## Prerequisites

- API must be running on `http://localhost:5041`
- PowerShell 5.1 or higher

## Scripts

### 01-get-btc-data.ps1
**Purpose:** Fetch current live Bitcoin price data  
**HTTP Method:** GET  
**Endpoint:** `/api/BtcData`  
**Description:** Retrieves real-time BTC-EUR price from CoinDesk API and converts to CZK using CNB exchange rates.

### 02-save-snapshot.ps1
**Purpose:** Save current Bitcoin data as a snapshot  
**HTTP Method:** POST  
**Endpoint:** `/api/Snapshots/Save`  
**Description:** Fetches current BTC data and saves it with a timestamped note. Demonstrates the full snapshot creation process.

### 03-get-all-snapshots.ps1
**Purpose:** List all saved snapshots  
**HTTP Method:** GET  
**Endpoint:** `/api/Snapshots/All`  
**Description:** Returns a list of all snapshots with their IDs and notes, ordered by ID descending (newest first).

### 04-get-snapshot-by-id.ps1
**Purpose:** Retrieve a specific snapshot  
**HTTP Method:** GET  
**Endpoint:** `/api/Snapshots/{id}`  
**Description:** Gets full details of a snapshot including its ID, note, and all data records. Edit the `$snapshotId` variable before running.

### 05-update-snapshot-note.ps1
**Purpose:** Update a snapshot's note  
**HTTP Method:** PUT  
**Endpoint:** `/api/Snapshots/{id}/note`  
**Description:** Changes the note text for an existing snapshot. Edit `$snapshotId` and `$newNote` variables before running.

### 06-delete-snapshot.ps1
**Purpose:** Delete a snapshot permanently  
**HTTP Method:** DELETE  
**Endpoint:** `/api/Snapshots/{id}`  
**Description:** Removes a snapshot and all its data from the database. Includes confirmation prompt. Edit the `$snapshotId` variable before running.

### 07-get-last-snapshot.ps1
**Purpose:** Get the most recently saved snapshot  
**HTTP Method:** GET  
**Endpoint:** `/api/Snapshots/Last`  
**Description:** Returns the latest snapshot with full details, or null if no snapshots exist.

## Usage

1. Ensure the API is running:
   ```powershell
   cd BitcoinCourse/BitcoinCourseAPI
   dotnet run
   ```

2. Navigate to the api-examples folder:
   ```powershell
   cd api-examples
   ```

3. Run any script:
   ```powershell
   .\01-get-btc-data.ps1
   ```

## Customization

For scripts that require IDs or custom data (04, 05, 06), edit the variables at the top of each script before running:
- `$snapshotId` - The ID of the snapshot to operate on
- `$newNote` - Custom note text for updates

## Troubleshooting

- **Connection errors**: Verify the API is running on port 5041
- **404 errors**: Check that the snapshot ID exists using script 03
- **400 errors**: Ensure required fields (like notes) are not empty
