# Save a snapshot with a custom note
# First gets current BTC data, then saves it as a snapshot

$apiUrl = "http://localhost:5041/api/BtcData"
$saveUrl = "http://localhost:5041/api/Snapshots/Save"

try {
    # Get current BTC data
    Write-Host "Fetching current Bitcoin data..." -ForegroundColor Yellow
    $btcData = Invoke-RestMethod -Uri $apiUrl -Method Get -ContentType "application/json"
    
    # Prepare snapshot request with custom note
    $snapshotRequest = @{
        Note = "Test snapshot - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        Records = $btcData
    }
    
    $jsonBody = $snapshotRequest | ConvertTo-Json -Depth 10
    
    # Save snapshot
    Write-Host "Saving snapshot..." -ForegroundColor Yellow
    $response = Invoke-RestMethod -Uri $saveUrl -Method Post -Body $jsonBody -ContentType "application/json"
    
    Write-Host "Snapshot saved successfully!" -ForegroundColor Green
    Write-Host "Response: $($response | ConvertTo-Json)" -ForegroundColor Green
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
}
