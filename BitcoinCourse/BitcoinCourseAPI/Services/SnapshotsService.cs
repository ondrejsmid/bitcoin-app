using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinCourseAPI.Services
{
    public interface ISnapshotsService
    {
        Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records);
    }

    public class SnapshotsService : ISnapshotsService
    {
        public Task<IActionResult> SaveRecordsAsync(List<BtcRecord>? records)
        {
            throw new NotImplementedException();
        }
    }
}