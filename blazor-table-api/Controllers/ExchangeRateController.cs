using BlazorTableApi.Models;
using BlazorTableApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTableApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRateController : ControllerBase
{
    private readonly CnbExchangeRateService _cnbService;

    public ExchangeRateController(CnbExchangeRateService cnbService)
    {
        _cnbService = cnbService;
    }

    [HttpGet("eur-czk")]
    public async Task<IActionResult> GetEurToCzk()
    {
        var rate = await _cnbService.GetEurToCzkRateAsync();
        if (rate.HasValue)
        {
            return Ok(new ExchangeRateDto
            {
                Rate = rate.Value,
                CurrencyCode = "EUR",
                TargetCode = "CZK",
                FetchedAt = DateTime.UtcNow
            });
        }

        return StatusCode(502, "Failed to fetch EUR->CZK rate from CNB");
    }
}
