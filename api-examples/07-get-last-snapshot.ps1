# Get the most recent snapshot
# Returns null if no snapshots exist

$apiUrl = "http://localhost:5041/api/Snapshots/Last"

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Get -ContentType "application/json"
    
    if ($null -eq $response) {
        Write-Host "No snapshots found." -ForegroundColor Yellow
    }
    else {
        Write-Host "Last Snapshot:" -ForegroundColor Green
        Write-Host "==============" -ForegroundColor Green
        Write-Host "ID: $($response.id)"
        Write-Host "Note: $($response.note)"
        Write-Host "`nData:" -ForegroundColor Cyan
        
        $response.data | ForEach-Object {
            Write-Host "  $($_.fieldName): $($_.value)"
        }
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
}
