﻿using Microsoft.Extensions.Logging;
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

            if (priceRange.TypeCode == '1')
            {
                allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.RCName == priceRange.RCName && r.FinQualityLevelCode == priceRange.FinQualityLevelCode &&
                  r.Action.Contains("Strategy_") == false && r.IsArch == '0').ToListAsync();
            }
            else if (priceRange.TypeCode == '2' && priceRange.RCName != null) {
                allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.RCName == priceRange.RCName && r.Action.Contains("Strategy_") == false && r.IsArch == '0').ToListAsync();
            }
            else if (priceRange.TypeCode == '2')
            {
                var wallMaterialCodeGF = new WallMaterialReference();
                if (priceRange.WallMaterialCode != null)
                {
                    int type = priceRange.CityCodeKATO == "471010" ? 3 : priceRange.CityCodeKATO == "71" ? 2 : 1;
                    wallMaterialCodeGF = await _context.WallMaterialReferences.FirstOrDefaultAsync(r => r.WallMaterialCodeColvir == priceRange.WallMaterialCode && r.Type == type);
                }

                if (priceRange.TypeEstateCode == "001" && priceRange.ApartmentLayoutCode != null)
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode && r.TypeEstateCode == priceRange.TypeEstateCode &&
                 r.ApartmentLayoutCode == priceRange.ApartmentLayoutCode && r.WallMaterialCode == wallMaterialCodeGF.WallMaterialCodeGF && r.IsArch == '0'
                 && r.TypeCode == priceRange.TypeCode && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
                else if (priceRange.TypeEstateCode == "002" && priceRange.DetailAreaCode != null)
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode && r.TypeEstateCode == priceRange.TypeEstateCode
                   && r.DetailAreaCode == priceRange.DetailAreaCode && r.IsArch == '0' &&
                 r.TypeCode == priceRange.TypeCode && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
                else if (priceRange.TypeEstateCode == "003")
                {
                    allList = await _context.LogData.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.SectorCode == priceRange.SectorCode &&
                    r.TypeEstateCode == priceRange.TypeEstateCode && r.IsArch == '0' && r.TypeCode == priceRange.TypeCode
                    && r.Action.Contains("Strategy_") == false).ToListAsync();
                }
            }

            var priceRequest=new LogData();

            if (allList.Count > 1 && priceRange.TypeCode == '2' && priceRange.RCName != null) {
                priceRequest = allList.OrderByDescending(r => r.MaxCostPerSQM).First();
            }
            else if (allList.Count > 1) return StatusCode(406, "Вернул больше 1 диапазона");
            else if (allList.Count == 0) return StatusCode(406, "По входящим параметрам нет диапазона");

            var data = new { MinCostPerSQM = allList.OrderByDescending(r => r.MaxCostPerSQM).First().MinCostPerSQM, MaxCostPerSQM = allList.OrderByDescending(r => r.MaxCostPerSQM).First().MaxCostPerSQM };

            priceRequest = allList.OrderByDescending(r => r.MaxCostPerSQM).First();
            priceRequest.Action = "Strategy_"+priceRange.NameStrategy;
            priceRequest.Id = 0;
            priceRequest.IsArch = '1';

            _context.LogData.Add(priceRequest);

            await _context.SaveChangesAsync();

            return Ok(data);
        }

    }
}