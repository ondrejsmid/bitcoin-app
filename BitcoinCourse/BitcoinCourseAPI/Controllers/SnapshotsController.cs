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
        public async Task<IActionResult> Save([FromBody]  List<BtcRecord> records)
        {
            await _snapshotsService.SaveRecordsAsync(records);
            return Ok();
        }

        [HttpGet("Last")]
        public async Task<IActionResult> GetLast()
        {
            var lastSnapshot = await _snapshotsService.GetLastSnapshotAsync();
            
            if (lastSnapshot == null)
            {
                return Ok(null);
            }

            return Ok(lastSnapshot);
        }
    }
}
