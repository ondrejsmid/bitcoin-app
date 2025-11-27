using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitcoinCourseAPI.Data;

namespace BitcoinCourseAPI.Services
{
    public interface ISnapshotsService
    {
        Task SaveRecordsAsync(List<BtcRecord>? records);
    }

    public class SnapshotsService : ISnapshotsService
    {
        private readonly BitcoinDbContext _db;

        public SnapshotsService(BitcoinDbContext db)
        {
            _db = db;
        }

        public async Task SaveRecordsAsync(List<BtcRecord>? records)
        {
            // Minimal EF insert: add a single dummy row into dbo.snapshot
            await _db.Database.ExecuteSqlRawAsync("INSERT INTO dbo.snapshot (note) VALUES ('dummy');");
        }
    }
}