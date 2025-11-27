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
            await _snapshotsService.SaveRecordsAsync(new List<BtcRecord>() { new BtcRecord { FieldName = "aa", Value = "11" }, new BtcRecord { FieldName = "bb", Value = "22" } });
            return Ok();
        }
    }
}
