using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bcc.Pledg.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StrategyController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<StrategyController> _logger;

        public StrategyController(PostgresContext context, ILogger<StrategyController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpPost("priceRange")]
        public async Task<ActionResult> PriceRangeRisk([FromBody] PriceRangeSecured priceRange)
        {
            var allList = new List<LogData>();

            if (priceRange.TypeCode == '1') {
                allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.RCName == priceRange.RCName && r.FinQualityLevelCode == priceRange.FinQualityLevelCode &&
                  r.Action.Contains("Strategy_") == false && r.IsArch == '0').ToListAsync();
            }
            else if (priceRange.TypeCode == '2') {
                if (priceRange.TypeEstateCode == "001" && priceRange.ApartmentLayoutCode != null)
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode && r.TypeEstateCode == priceRange.TypeEstateCode &&
                 r.ApartmentLayoutCode == priceRange.ApartmentLayoutCode && r.WallMaterialCode == priceRange.WallMaterialCode && r.IsArch == '0'
                 && r.TypeCode == priceRange.TypeCode && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
                else if (priceRange.TypeEstateCode == "002" && priceRange.ApartmentLayoutCode != null)
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode && r.TypeEstateCode == priceRange.TypeEstateCode
                 && r.WallMaterialCode == priceRange.WallMaterialCode && r.DetailAreaCode == priceRange.DetailAreaCode && r.IsArch == '0' &&
                 r.TypeCode == priceRange.TypeCode && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
                else if (priceRange.TypeEstateCode == "003")
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode &&
                    r.TypeEstateCode == priceRange.TypeEstateCode && r.IsArch == '0' && r.TypeCode == priceRange.TypeCode
                    && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
            }

            if (allList.Count > 1) return StatusCode(406, "Вернул больше 1 диапазона");
            else if (allList.Count == 0) return StatusCode(406, "По входящим параметрам нет диапазона");

            var data = new { MinCostPerSQM = allList[0].MinCostPerSQM, MaxCostPerSQM = allList[0].MaxCostPerSQM };

            var priceRequest = allList[0];
            priceRequest.Action = "Strategy_"+priceRange.NameStrategy;
            priceRequest.Id = 0;

            _context.LogData.Add(priceRequest);

            await _context.SaveChangesAsync();

            return Ok(data);
        }

    }
}