# Delete a snapshot by ID
# WARNING: This permanently deletes the snapshot and all its data
# Replace $snapshotId with the ID you want to delete

$snapshotId = 1  # Change this to the snapshot ID you want to delete
$apiUrl = "http://localhost:5041/api/Snapshots/$snapshotId"

# Confirmation prompt
$confirmation = Read-Host "Are you sure you want to delete snapshot ID $snapshotId? (yes/no)"

if ($confirmation -eq "yes") {
    try {
        Write-Host "Deleting snapshot $snapshotId..." -ForegroundColor Yellow
        Invoke-RestMethod -Uri $apiUrl -Method Delete -ContentType "application/json"
        
        Write-Host "Snapshot deleted successfully!" -ForegroundColor Green
    }
    catch {
        Write-Host "Error: $_" -ForegroundColor Red
        Write-Host "Make sure the snapshot ID exists." -ForegroundColor Yellow
    }
}
else {
    Write-Host "Deletion cancelled." -ForegroundColor Yellow
}
