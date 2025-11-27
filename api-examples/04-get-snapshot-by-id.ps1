# Get a specific snapshot by ID
# Replace $snapshotId with the actual ID you want to retrieve

$snapshotId = 1  # Change this to the desired snapshot ID
$apiUrl = "http://localhost:5041/api/Snapshots/$snapshotId"

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Get -ContentType "application/json"
    
    Write-Host "Snapshot Details:" -ForegroundColor Green
    Write-Host "=================" -ForegroundColor Green
    Write-Host "ID: $($response.id)"
    Write-Host "Note: $($response.note)"
    Write-Host "`nData:" -ForegroundColor Cyan
    
    $response.data | ForEach-Object {
        Write-Host "  $($_.fieldName): $($_.value)"
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host "Make sure the snapshot ID exists." -ForegroundColor Yellow
}
