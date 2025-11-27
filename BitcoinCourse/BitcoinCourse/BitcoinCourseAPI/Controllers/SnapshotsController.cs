using BitcoinCourseAPI.Data;
using BitcoinCourseAPI.Models;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinCourseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SnapshotsController : Controller
    {
        private readonly BitcoinDbContext _db;

        public SnapshotsController(BitcoinDbContext db)
        {
            _db = db;
        }

        [HttpPost("Save")]
        //public async Task<IActionResult> Save([FromBody] List<BtcRecord> records)
        public async Task<IActionResult> Save()
        {
            return await SaveRecordsAsync(records);
        }
      
        private async Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records)
        {
            //if (records == null || records.Count == 0)
            //    return BadRequest("No records provided");

            //var snapshot = new Snapshot();
            //_db.Snapshots.Add(snapshot);
            //await _db.SaveChangesAsync();

            //foreach (var r in records)
            //{
            //    _db.SnapshotRows.Add(new SnapshotRow
            //    {
            //        SnapshotId = snapshot.Id,
            //        BtcFieldName = r.FieldName ?? string.Empty,
            //        BtcFieldValue = r.Value ?? string.Empty
            //    });
            //}

            //await _db.SaveChangesAsync();

            //return Ok(new { snapshotId = snapshot.Id });
            return Ok(new { snapshotId = 123 });
        }
    }
}
