using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BlazorTableApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BlazorTableApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitcoinDataController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BitcoinDataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var connStr = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection")
                          ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                          ?? "Server=localhost,1433;Database=BitcoinAppDb;User Id=sa;Password=12Panacek!;TrustServerCertificate=True;";

            var results = new List<BitcoinDataDto>();

            try
            {
                await using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT TOP 100 Id, [Key], [Value], CreatedAt FROM BitcoinData ORDER BY CreatedAt DESC, Id DESC", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(new BitcoinDataDto
                    {
                        Id = reader.GetInt32(0),
                        Key = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Value = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CreatedAt = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3)
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to read BitcoinData: {ex.Message}");
            }

            return Ok(results);
        }
    }
}
