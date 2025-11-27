using BitcoinCourseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinCourseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BtcDataController : Controller
    {
        private readonly IBtcDataService _btcDataService;

        public BtcDataController(IBtcDataService btcDataService)
        {
            _btcDataService = btcDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBtcDataAsync()
        {
            return Ok(await _btcDataService.GetBtcData());
        }
    }
}
