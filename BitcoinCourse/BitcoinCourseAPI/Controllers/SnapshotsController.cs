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
        public async Task<IActionResult> Save([FromBody] SaveSnapshotRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Note))
            {
                return BadRequest("Note is required");
            }

            await _snapshotsService.SaveRecordsAsync(request.Records, request.Note);
            return Ok();
        }

        [HttpGet("Last")]
        public async Task<IActionResult> GetLast()
        {
            var lastSnapshot = await _snapshotsService.GetLastSnapshotAsync();
            
            if (lastSnapshot == null)
            {
                return Ok(null); // Returns null in JSON response
            }

            return Ok(lastSnapshot);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var snapshots = await _snapshotsService.GetAllSnapshotsAsync();
            return Ok(snapshots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var snapshot = await _snapshotsService.GetSnapshotByIdAsync(id);
            
            if (snapshot == null)
            {
                return NotFound();
            }

            return Ok(snapshot);
        }
    }
}
