# Get current Bitcoin data from API
# This endpoint fetches live BTC-EUR price from CoinDesk and converts to CZK

$apiUrl = "http://localhost:5041/api/BtcData"

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Get -ContentType "application/json"
    
    Write-Host "Bitcoin Data Retrieved Successfully:" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Green
    
    $response | ForEach-Object {
        Write-Host "$($_.fieldName): $($_.value)"
    }
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
}
