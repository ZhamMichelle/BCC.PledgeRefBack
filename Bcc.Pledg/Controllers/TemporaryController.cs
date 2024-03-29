﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bcc.Pledg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Bcc.Pledg.Controllers
{

    [Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class TemporaryController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<TemporaryController> _logger;

        public string City { get; private set; }

        public TemporaryController(PostgresContext context, ILogger<TemporaryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("existCities")]
        public async Task<List<string>> GetExistCities() {
            
            var arr = await _context.PledgeRefs.Select(x=>new { x.City }).Distinct().ToListAsync();
            List<string> existCities = new List<string>();

            if (arr != null)
            {
                foreach (var item in arr)
                {
                    existCities.Add(item.City);
                }
            }
            else existCities.Add("Не загружены города");

            return existCities;
        }

        [HttpGet("city")]
        public async Task<ActionResult> GetCity(string city)
        {
            var allList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
            return Ok(allList);
        }
        [HttpGet("priceRange")]
        public async Task<ActionResult> PriceRange([FromBody] PriceRange priceRange)
        {
            var allList = new List<PledgeReference>();


            if (priceRange.TypeEstateCode == "001" && priceRange.ApartmentLayoutCode != null)
            {
                allList = await _context.PledgeRefs.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.Sector == priceRange.Sector && r.TypeEstateCode == priceRange.TypeEstateCode &&
             r.ApartmentLayoutCode == priceRange.ApartmentLayoutCode && r.WallMaterialCode == priceRange.WallMaterialCode).ToListAsync();
            }
            else if (priceRange.TypeEstateCode == "002" && priceRange.ApartmentLayoutCode != null)
            {
                allList = await _context.PledgeRefs.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.Sector == priceRange.Sector && r.TypeEstateCode == priceRange.TypeEstateCode
             && r.WallMaterialCode == priceRange.WallMaterialCode && r.DetailAreaCode == priceRange.DetailAreaCode).ToListAsync();
            }
            else if (priceRange.TypeEstateCode == "003")
            {
                allList = await _context.PledgeRefs.Where(r => r.CityCodeKATO == priceRange.CityCodeKATO && r.Sector == priceRange.Sector && r.TypeEstateCode == priceRange.TypeEstateCode).ToListAsync();
            }

            if (allList.Count > 1) return StatusCode(406, "Вернул больше 1 диапазона");
            else if (allList.Count == 0) return StatusCode(406, "По входящим параметрам нет диапазона");

            var data = new { MinCostPerSQM = allList[0].MinCostPerSQM, MaxCostPerSQM = allList[0].MaxCostPerSQM };

            return Ok(data);
        }


        [HttpGet("search/sector")]
        public async Task<ActionResult> GetBySearchSector(string city, string sector)
        {
            if (sector != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch(string city, string sector, string estate)
        {
            if (sector != null && estate != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector && r.TypeEstateByRef.ToLower().Contains(estate.ToLower())).ToListAsync();
                return Ok(searchList);
            }
            else if (sector == null && estate != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.TypeEstateByRef.ToLower().Contains(estate.ToLower())).ToListAsync();
                return Ok(searchList);
            }
            else if (sector != null && estate == null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveId(int id, [FromQuery]string username)
        {

            var deleteParams = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteParams == null)
                return NotFound();
            _context.PledgeRefs.Remove(deleteParams);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = deleteParams.Code,
                CityCodeKATO = deleteParams.CityCodeKATO,
                City = deleteParams.City,
                SectorCode = deleteParams.SectorCode,
                Sector = deleteParams.Sector,
                RelativityLocation = deleteParams.RelativityLocation,
                SectorDescription = deleteParams.SectorDescription,
                TypeEstateCode = deleteParams.TypeEstateCode,
                TypeEstateByRef = deleteParams.TypeEstateByRef,
                ApartmentLayoutCode = deleteParams.ApartmentLayoutCode,
                ApartmentLayout = deleteParams.ApartmentLayout,
                WallMaterialCode = deleteParams.WallMaterialCode,
                WallMaterial = deleteParams.WallMaterial,
                DetailAreaCode = deleteParams.DetailAreaCode,
                DetailArea = deleteParams.DetailArea,
                MinCostPerSQM = deleteParams.MinCostPerSQM,
                MaxCostPerSQM = deleteParams.MaxCostPerSQM,
                Bargain = deleteParams.Bargain,
                MinCostWithBargain = deleteParams.MinCostWithBargain,
                MaxCostWithBargain = deleteParams.MaxCostWithBargain,
                BeginDate = deleteParams.BeginDate,
                EndDate = deleteParams.EndDate,
                Action = "Удаление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch='1',
                TypeCode = '1',
                Type = "Первичка",
            });

            await _context.SaveChangesAsync();
            return Ok("Removed");
        }

        [HttpDelete("city/{city}")]
        public async Task<ActionResult> RemoveCity(string city, [FromQuery]string username)
        {

            var deleteParams = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
            if (deleteParams == null)
                return NotFound();
            _context.PledgeRefs.RemoveRange(deleteParams);
            foreach (var item in deleteParams)
            {
                var logdata = _context.LogData.Add(new LogData()
                {
                    Code = item.Code,
                    CityCodeKATO = item.CityCodeKATO,
                    City = item.City,
                    SectorCode = item.SectorCode,
                    Sector = item.Sector,
                    RelativityLocation = item.RelativityLocation,
                    SectorDescription = item.SectorDescription,
                    TypeEstateCode = item.TypeEstateCode,
                    TypeEstateByRef = item.TypeEstateByRef,
                    ApartmentLayoutCode = item.ApartmentLayoutCode,
                    ApartmentLayout = item.ApartmentLayout,
                    WallMaterialCode = item.WallMaterialCode,
                    WallMaterial = item.WallMaterial,
                    DetailAreaCode = item.DetailAreaCode,
                    DetailArea = item.DetailArea,
                    MinCostPerSQM = item.MinCostPerSQM,
                    MaxCostPerSQM = item.MaxCostPerSQM,
                    Bargain = item.Bargain,
                    MinCostWithBargain = item.MinCostWithBargain,
                    MaxCostWithBargain = item.MaxCostWithBargain,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    Action = "Удаление по городу",
                    Username = username,
                    ChangeDate = DateTime.Today,
                    IsArch='1',
                    TypeCode = '1',
                    Type = "Первичка",
                });

                await _context.SaveChangesAsync();
            }
            return Ok("Removed");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id)
        {
            var getElement = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (getElement == null)
                return NotFound();
            return Ok(getElement);
        }

        [HttpPost]
        public async Task<ActionResult> AddElement([FromBody]PledgeReference analysis, [FromQuery]string username)
        {
            _context.PledgeRefs.Add(analysis);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = analysis.Code,
                CityCodeKATO = analysis.CityCodeKATO,
                City = analysis.City,
                SectorCode = analysis.SectorCode,
                Sector = analysis.Sector,
                RelativityLocation = analysis.RelativityLocation,
                SectorDescription = analysis.SectorDescription,
                TypeEstateCode = analysis.TypeEstateCode,
                TypeEstateByRef = analysis.TypeEstateByRef,
                ApartmentLayoutCode = analysis.ApartmentLayoutCode,
                ApartmentLayout = analysis.ApartmentLayout,
                WallMaterialCode = analysis.WallMaterialCode,
                WallMaterial = analysis.WallMaterial,
                DetailAreaCode = analysis.DetailAreaCode,
                DetailArea = analysis.DetailArea,
                MinCostPerSQM = analysis.MinCostPerSQM,
                MaxCostPerSQM = analysis.MaxCostPerSQM,
                Bargain = analysis.Bargain,
                MinCostWithBargain = analysis.MinCostWithBargain,
                MaxCostWithBargain = analysis.MaxCostWithBargain,
                BeginDate = analysis.BeginDate,
                EndDate = analysis.EndDate,
                Action = "Добавление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch='0',
                TypeCode = '1',
                Type = "Первичка",
            });

            await _context.SaveChangesAsync();

            return Ok("Added");
        }

        [HttpPut]
        public async Task<ActionResult> PutElement([FromBody] PledgeReference analysis, [FromQuery]string username)
        {
            var result = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CityCodeKATO = analysis.CityCodeKATO;
                    result.City = analysis.City;
                    result.SectorCode = analysis.SectorCode;
                    result.Sector = analysis.Sector;
                    result.RelativityLocation = analysis.RelativityLocation;
                    result.SectorDescription = analysis.SectorDescription;
                    result.TypeEstateCode = analysis.TypeEstateCode;
                    result.TypeEstateByRef = analysis.TypeEstateByRef;
                    result.ApartmentLayoutCode = analysis.ApartmentLayoutCode;
                    result.ApartmentLayout = analysis.ApartmentLayout;
                    result.WallMaterialCode = analysis.WallMaterialCode;
                    result.WallMaterial = analysis.WallMaterial;
                    result.DetailAreaCode = analysis.DetailAreaCode;
                    result.DetailArea = analysis.DetailArea;
                    result.MinCostPerSQM = analysis.MinCostPerSQM;
                    result.MaxCostPerSQM = analysis.MaxCostPerSQM;
                    result.Bargain = analysis.Bargain;
                    result.MinCostWithBargain = analysis.MinCostWithBargain;
                    result.MaxCostWithBargain = analysis.MaxCostWithBargain;
                    result.BeginDate = analysis.BeginDate;
                    result.EndDate = analysis.EndDate;


                    var logdata = _context.Add(new LogData()
                    {
                        Code = analysis.Code,
                        CityCodeKATO = analysis.CityCodeKATO,
                        City = analysis.City,
                        SectorCode = analysis.SectorCode,
                        Sector = analysis.Sector,
                        RelativityLocation = analysis.RelativityLocation,
                        SectorDescription = analysis.SectorDescription,
                        TypeEstateCode = analysis.TypeEstateCode,
                        TypeEstateByRef = analysis.TypeEstateByRef,
                        ApartmentLayoutCode = analysis.ApartmentLayoutCode,
                        ApartmentLayout = analysis.ApartmentLayout,
                        WallMaterialCode = analysis.WallMaterialCode,
                        WallMaterial = analysis.WallMaterial,
                        DetailAreaCode = analysis.DetailAreaCode,
                        DetailArea = analysis.DetailArea,
                        MinCostPerSQM = analysis.MinCostPerSQM,
                        MaxCostPerSQM = analysis.MaxCostPerSQM,
                        Bargain = analysis.Bargain,
                        MinCostWithBargain = analysis.MinCostWithBargain,
                        MaxCostWithBargain = analysis.MaxCostWithBargain,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Редактирование",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch='0',
                        TypeCode = '1',
                        Type = "Первичка",
                    });

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) {
                _logger.LogError("Error", e);
            }

            return Ok();
        }


        [HttpPut("archandnew")]
        public async Task<ActionResult> ArchAndNewElement([FromBody] PledgeReference analysis, [FromQuery]string username)
        {
            var result = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CityCodeKATO = analysis.CityCodeKATO;
                    result.City = analysis.City;
                    result.SectorCode = analysis.SectorCode;
                    result.Sector = analysis.Sector;
                    result.RelativityLocation = analysis.RelativityLocation;
                    result.SectorDescription = analysis.SectorDescription;
                    result.TypeEstateCode = analysis.TypeEstateCode;
                    result.TypeEstateByRef = analysis.TypeEstateByRef;
                    result.ApartmentLayoutCode = analysis.ApartmentLayoutCode;
                    result.ApartmentLayout = analysis.ApartmentLayout;
                    result.WallMaterialCode = analysis.WallMaterialCode;
                    result.WallMaterial = analysis.WallMaterial;
                    result.DetailAreaCode = analysis.DetailAreaCode;
                    result.DetailArea = analysis.DetailArea;
                    result.MinCostPerSQM = analysis.MinCostPerSQM;
                    result.MaxCostPerSQM = analysis.MaxCostPerSQM;
                    result.Bargain = analysis.Bargain;
                    result.MinCostWithBargain = analysis.MinCostWithBargain;
                    result.MaxCostWithBargain = analysis.MaxCostWithBargain;
                    result.BeginDate = analysis.BeginDate;
                    result.EndDate = analysis.EndDate;

                    var oldData = _context.LogData.Where(f => f.Code == analysis.Code && f.EndDate==null).ToList();
                    foreach (var item in oldData)
                    {
                        item.EndDate = analysis.BeginDate.Value.AddDays(-1);
                        item.IsArch = '1';
                    }

                    var logdata = _context.Add(new LogData()
                    {
                        Code = analysis.Code,
                        CityCodeKATO = analysis.CityCodeKATO,
                        City = analysis.City,
                        SectorCode = analysis.SectorCode,
                        Sector = analysis.Sector,
                        RelativityLocation = analysis.RelativityLocation,
                        SectorDescription = analysis.SectorDescription,
                        TypeEstateCode = analysis.TypeEstateCode,
                        TypeEstateByRef = analysis.TypeEstateByRef,
                        ApartmentLayoutCode = analysis.ApartmentLayoutCode,
                        ApartmentLayout = analysis.ApartmentLayout,
                        WallMaterialCode = analysis.WallMaterialCode,
                        WallMaterial = analysis.WallMaterial,
                        DetailAreaCode = analysis.DetailAreaCode,
                        DetailArea = analysis.DetailArea,
                        MinCostPerSQM = analysis.MinCostPerSQM,
                        MaxCostPerSQM = analysis.MaxCostPerSQM,
                        Bargain = analysis.Bargain,
                        MinCostWithBargain = analysis.MinCostWithBargain,
                        MaxCostWithBargain = analysis.MaxCostWithBargain,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Добавление",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch='0',
                        TypeCode = '1',
                        Type = "Первичка",
                    });

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error", e);
            }

            return Ok();
        }
    }
}
