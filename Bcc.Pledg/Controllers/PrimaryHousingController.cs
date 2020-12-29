using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Bcc.Pledg.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bcc.Pledg.Controllers
{
    [Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class PrimaryHousingController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<PrimaryHousingController> _logger;

        public PrimaryHousingController(PostgresContext context, ILogger<PrimaryHousingController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("city")]
        public async Task<ActionResult> GetCity([FromQuery]string city)
        {
            var allList = await _context.PrimaryPledgeRefs.Where(r => r.City == city).ToListAsync();
            return Ok(allList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id)
        {
            var getElement = await _context.PrimaryPledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (getElement == null)
                return NotFound();
            return Ok(getElement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveId(int id, [FromQuery]string username)
        {

            var deleteParams = await _context.PrimaryPledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteParams == null)
                return NotFound();
            _context.PrimaryPledgeRefs.Remove(deleteParams);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = deleteParams.Code,
                CityCodeKATO = deleteParams.CityCodeKATO,
                City = deleteParams.City,
                RCNameCode = deleteParams.RCNameCode,
                RCName = deleteParams.RCName,
                ActualAdress = deleteParams.ActualAdress,
                FinQualityLevelCode = deleteParams.FinQualityLevelCode,
                FinQualityLevel = deleteParams.FinQualityLevel,
                MinCostPerSQM = deleteParams.MinCostPerSQM,
                MaxCostPerSQM = deleteParams.MaxCostPerSQM,
                BeginDate = deleteParams.BeginDate,
                EndDate = deleteParams.EndDate,
                Action = "Удаление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch = '1',
                TypeCode='2',
                Type = "Вторичка",

            });

            await _context.SaveChangesAsync();
            return Ok("Removed");
        }

        [HttpDelete("city/{city}")]
        public async Task<ActionResult> RemoveCity(string city, [FromQuery]string username)
        {

            var deleteParams = await _context.PrimaryPledgeRefs.Where(r => r.City == city).ToListAsync();
            if (deleteParams == null)
                return NotFound();
            _context.PrimaryPledgeRefs.RemoveRange(deleteParams);
            foreach (var item in deleteParams)
            {
                var logdata = _context.LogData.Add(new LogData()
                {
                    Code = item.Code,
                    CityCodeKATO = item.CityCodeKATO,
                    City = item.City,
                    RCNameCode = item.RCNameCode,
                    RCName = item.RCName,
                    ActualAdress = item.ActualAdress,
                    FinQualityLevelCode = item.FinQualityLevelCode,
                    FinQualityLevel = item.FinQualityLevel,
                    MinCostPerSQM = item.MinCostPerSQM,
                    MaxCostPerSQM = item.MaxCostPerSQM,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    Action = "Удаление по городу",
                    Username = username,
                    ChangeDate = DateTime.Today,
                    IsArch = '1',
                    TypeCode='2',
                    Type = "Вторичка",
                });

                await _context.SaveChangesAsync();
            }
            return Ok("Removed");
        }

        [HttpPost]
        public async Task<ActionResult> AddElement([FromBody] PrimaryPledgeRef analysis, [FromQuery]string username)
        {
            _context.PrimaryPledgeRefs.Add(analysis);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = analysis.Code,
                CityCodeKATO = analysis.CityCodeKATO,
                City = analysis.City,
                RCNameCode = analysis.RCNameCode,
                RCName = analysis.RCName,
                ActualAdress = analysis.ActualAdress,
                FinQualityLevelCode = analysis.FinQualityLevelCode,
                FinQualityLevel = analysis.FinQualityLevel,
                MinCostPerSQM = analysis.MinCostPerSQM,
                MaxCostPerSQM = analysis.MaxCostPerSQM,
                BeginDate = analysis.BeginDate,
                EndDate = analysis.EndDate,
                Action = "Добавление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch = '0',
                TypeCode = '2',
                Type = "Вторичка",
            });

            await _context.SaveChangesAsync();

            return Ok("Added");
        }


        [HttpPut]
        public async Task<ActionResult> PutElement([FromBody] PrimaryPledgeRef analysis, [FromQuery]string username)
        {
            var result = await _context.PrimaryPledgeRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CityCodeKATO = analysis.CityCodeKATO;
                    result.City = analysis.City;
                    result.RCNameCode = analysis.RCNameCode;
                    result.RCName = analysis.RCName;
                    result.ActualAdress = analysis.ActualAdress;
                    result.FinQualityLevelCode = analysis.FinQualityLevelCode;
                    result.FinQualityLevel = analysis.FinQualityLevel;
                    result.MinCostPerSQM = analysis.MinCostPerSQM;
                    result.MaxCostPerSQM = analysis.MaxCostPerSQM;
                    result.BeginDate = analysis.BeginDate;
                    result.EndDate = analysis.EndDate;


                    var logdata = _context.Add(new LogData()
                    {
                        Code = analysis.Code,
                        CityCodeKATO = analysis.CityCodeKATO,
                        City = analysis.City,
                        RCNameCode = analysis.RCNameCode,
                        RCName = analysis.RCName,
                        ActualAdress = analysis.ActualAdress,
                        FinQualityLevelCode = analysis.FinQualityLevelCode,
                        FinQualityLevel = analysis.FinQualityLevel,
                        MinCostPerSQM = analysis.MinCostPerSQM,
                        MaxCostPerSQM = analysis.MaxCostPerSQM,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Редактирование",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch = '0',
                        TypeCode = '2',
                        Type = "Вторичка",
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


        [HttpPut("archandnew")]
        public async Task<ActionResult> ArchAndNewElement([FromBody] PrimaryPledgeRef analysis, [FromQuery]string username)
        {
            var result = await _context.PrimaryPledgeRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CityCodeKATO = analysis.CityCodeKATO;
                    result.City = analysis.City;
                    result.RCNameCode = analysis.RCNameCode;
                    result.RCName = analysis.RCName;
                    result.ActualAdress = analysis.ActualAdress;
                    result.FinQualityLevelCode = analysis.FinQualityLevelCode;
                    result.FinQualityLevel = analysis.FinQualityLevel;
                    result.MinCostPerSQM = analysis.MinCostPerSQM;
                    result.MaxCostPerSQM = analysis.MaxCostPerSQM;
                    result.BeginDate = analysis.BeginDate;
                    result.EndDate = analysis.EndDate;

                    var oldData = _context.LogData.Where(f => f.Code == analysis.Code && f.EndDate == null).ToList();
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
                        RCNameCode = analysis.RCNameCode,
                        RCName = analysis.RCName,
                        ActualAdress = analysis.ActualAdress,
                        FinQualityLevelCode = analysis.FinQualityLevelCode,
                        FinQualityLevel = analysis.FinQualityLevel,
                        MinCostPerSQM = analysis.MinCostPerSQM,
                        MaxCostPerSQM = analysis.MaxCostPerSQM,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Добавление",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch = '0',
                        TypeCode = '2',
                        Type = "Вторичка",
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