using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitcoinCourseAPI.Data;
using BitcoinCourseAPI.Models;

namespace BitcoinCourseAPI.Services
{
    public interface ISnapshotsService
    {
        Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records);
        Task<LastSnapshotResponse?> GetLastSnapshotAsync();
    }

    public class SnapshotsService : ISnapshotsService
    {
        private readonly BitcoinDbContext _db;

        public SnapshotsService(BitcoinDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records)
        {
            if (records == null || records.Count == 0)
            {
                return new BadRequestObjectResult("No records provided");
            }

            // create snapshot
            var snapshot = new Snapshot { Note = "dummy" };
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
    }
}