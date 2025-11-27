using BitcoinCourseAPI.Services;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinCourseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SnapshotsController : Controller
    {
        private readonly ISnapshotsService _snapshotsService;

        public SnapshotsController(ISnapshotsService snapshotsService) => _snapshotsService = snapshotsService;

        [HttpPost("Save")]
        //public async Task<IActionResult> Save([FromBody] List<BtcRecord> records)
        public async Task<IActionResult> Save()
        {
            return await SaveRecordsAsync(new List<BtcRecord>() { new BtcRecord { FieldName = "aa", Value = "11" }, new BtcRecord { FieldName = "bb", Value = "22" } });
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
