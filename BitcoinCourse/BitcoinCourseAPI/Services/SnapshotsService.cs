using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitcoinCourseAPI.Data;
using BitcoinCourseAPI.Models;

namespace BitcoinCourseAPI.Services
{
    public interface ISnapshotsService
    {
        Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records, string? note);
        Task<LastSnapshotResponse?> GetLastSnapshotAsync();
        Task<List<SnapshotListItem>> GetAllSnapshotsAsync();
        Task<LastSnapshotResponse?> GetSnapshotByIdAsync(int id);
        Task<bool> UpdateSnapshotNoteAsync(int id, string note);
        Task<bool> DeleteSnapshotAsync(int id);
    }

    public class SnapshotsService : ISnapshotsService
    {
        private readonly BitcoinDbContext _db;

        public SnapshotsService(BitcoinDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records, string? note)
        {
            if (records == null || records.Count == 0)
            {
                return new BadRequestObjectResult("No records provided");
            }

            // create snapshot
            var snapshot = new Snapshot { Note = note ?? string.Empty };
            _db.Snapshots.Add(snapshot);
            await _db.SaveChangesAsync();

            // create rows linked to snapshot via snapshot.Id
            foreach (var r in records)
            {
                var row = new SnapshotRow
                {
                    SnapshotId = snapshot.Id,
                    BtcFieldName = r.FieldName ?? string.Empty,
                    BtcFieldValue = r.Value ?? string.Empty
                };
                _db.SnapshotRows.Add(row);
            }

            await _db.SaveChangesAsync();

            return new OkObjectResult(new { snapshotId = snapshot.Id });
        }

        public async Task<LastSnapshotResponse?> GetLastSnapshotAsync()
        {
            var lastSnapshot = await _db.Snapshots
                .Include(s => s.Rows)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (lastSnapshot == null)
            {
                return null;
            }

            var response = new LastSnapshotResponse
            {
                Id = lastSnapshot.Id,
                Note = lastSnapshot.Note,
                Data = lastSnapshot.Rows.Select(r => new BtcRecord
                {
                    FieldName = r.BtcFieldName,
                    Value = r.BtcFieldValue
                }).ToList()
            };

            return response;
        }

        public async Task<List<SnapshotListItem>> GetAllSnapshotsAsync()
        {
            var snapshots = await _db.Snapshots
                .OrderByDescending(s => s.Id)
                .Select(s => new SnapshotListItem
                {
                    Id = s.Id,
                    Note = s.Note
                })
                .ToListAsync();

            return snapshots;
        }

        public async Task<LastSnapshotResponse?> GetSnapshotByIdAsync(int id)
        {
            var snapshot = await _db.Snapshots
                .Include(s => s.Rows)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (snapshot == null)
            {
                return null;
            }

            var response = new LastSnapshotResponse
            {
                Id = snapshot.Id,
                Note = snapshot.Note,
                Data = snapshot.Rows.Select(r => new BtcRecord
                {
                    FieldName = r.BtcFieldName,
                    Value = r.BtcFieldValue
                }).ToList()
            };

            return response;
        }

        public async Task<bool> UpdateSnapshotNoteAsync(int id, string note)
        {
            var snapshot = await _db.Snapshots.FindAsync(id);
            
            if (snapshot == null)
            {
                return false;
            }

            snapshot.Note = note;
            await _db.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteSnapshotAsync(int id)
        {
            var snapshot = await _db.Snapshots
                .Include(s => s.Rows)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (snapshot == null)
            {
                return false;
            }

            _db.SnapshotRows.RemoveRange(snapshot.Rows);
            _db.Snapshots.Remove(snapshot);
            await _db.SaveChangesAsync();
            
            return true;
        }
    }
}