# Get list of all saved snapshots
# Returns ID and Note for each snapshot

$apiUrl = "http://localhost:5041/api/Snapshots/All"

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Get -ContentType "application/json"
    
    Write-Host "All Snapshots:" -ForegroundColor Green
    Write-Host "==============" -ForegroundColor Green
    
    if ($response.Count -eq 0) {
        Write-Host "No snapshots found." -ForegroundColor Yellow
    }
    else {
        $response | ForEach-Object {
            Write-Host "ID: $($_.id) | Note: $($_.note)"
        }
        Write-Host "`nTotal snapshots: $($response.Count)" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
}
