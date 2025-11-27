# Update the note of an existing snapshot
# Replace $snapshotId and $newNote with your values

$snapshotId = 1  # Change this to the snapshot ID you want to update
$newNote = "Updated note - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

$apiUrl = "http://localhost:5041/api/Snapshots/$snapshotId/note"

try {
    $jsonBody = $newNote | ConvertTo-Json
    
    Write-Host "Updating snapshot $snapshotId..." -ForegroundColor Yellow
    $response = Invoke-RestMethod -Uri $apiUrl -Method Put -Body $jsonBody -ContentType "application/json"
    
    Write-Host "Snapshot note updated successfully!" -ForegroundColor Green
    Write-Host "New note: $newNote" -ForegroundColor Cyan
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host "Make sure the snapshot ID exists and note is not empty." -ForegroundColor Yellow
}
