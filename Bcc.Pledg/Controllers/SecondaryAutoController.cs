using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bcc.Pledg.Models;
using Microsoft.EntityFrameworkCore;

namespace Bcc.Pledg.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecondaryAutoController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<SecondaryAutoController> _logger;

        public SecondaryAutoController(PostgresContext context, ILogger<SecondaryAutoController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getList")]
        public async Task<List<SecondaryAutoRef>> getList() {

            var autoList = await _context.SecondaryAutoRefs.ToListAsync();

            return autoList;
        }

        [HttpGet("{id}")]
        public async Task<SecondaryAutoRef> getId(int id) {
            var autoElement = await _context.SecondaryAutoRefs.FirstOrDefaultAsync(z => z.Id == id);
            return autoElement;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveId(int id, [FromQuery]string username)
        {

            var deleteParams = await _context.SecondaryAutoRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteParams == null)
                return NotFound();
            _context.SecondaryAutoRefs.Remove(deleteParams);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = deleteParams.Code,
                CarBrand = deleteParams.CarBrand,
                CarModel = deleteParams.CarModel,
                ProduceYear = deleteParams.ProduceYear,
                MarketCost = deleteParams.MarketCost,
                MaxPercentageDeviation = deleteParams.MaxPercentageDeviation,
                BeginDate = deleteParams.BeginDate,
                EndDate = deleteParams.EndDate,
                Action = "Удаление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch = '1',
                TypeCode = '3',
                Type = "Авто Вторичка",

            });

            await _context.SaveChangesAsync();
            return Ok("Removed");
        }

        [HttpPost]
        public async Task<ActionResult> AddElement([FromBody] SecondaryAutoRef analysis, [FromQuery]string username)
        {
            _context.SecondaryAutoRefs.Add(analysis);

            var logdata = _context.LogData.Add(new LogData()
            {
                Code = analysis.Code,
                CarBrand = analysis.CarBrand,
                CarModel = analysis.CarModel,
                ProduceYear = analysis.ProduceYear,
                MarketCost = analysis.MarketCost,
                MaxPercentageDeviation = analysis.MaxPercentageDeviation,
                BeginDate = analysis.BeginDate,
                EndDate = analysis.EndDate,
                Action = "Добавление",
                Username = username,
                ChangeDate = DateTime.Today,
                IsArch = '0',
                TypeCode = '3',
                Type = "Авто Вторичка",
            });

            await _context.SaveChangesAsync();

            return Ok("Added");
        }


        [HttpPut]
        public async Task<ActionResult> PutElement([FromBody] SecondaryAutoRef analysis, [FromQuery]string username)
        {
            var result = await _context.SecondaryAutoRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CarBrand = analysis.CarBrand;
                    result.CarModel = analysis.CarModel;
                    result.ProduceYear = analysis.ProduceYear;
                    result.MarketCost = analysis.MarketCost;
                    result.MaxPercentageDeviation = analysis.MaxPercentageDeviation;
                    result.BeginDate = analysis.BeginDate;
                    result.EndDate = analysis.EndDate;


                    var logdata = _context.Add(new LogData()
                    {
                        Code = analysis.Code,
                        CarBrand = analysis.CarBrand,
                        CarModel = analysis.CarModel,
                        ProduceYear = analysis.ProduceYear,
                        MarketCost = analysis.MarketCost,
                        MaxPercentageDeviation = analysis.MaxPercentageDeviation,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Редактирование",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch = '0',
                        TypeCode = '3',
                        Type = "Авто Вторичка",
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
        public async Task<ActionResult> ArchAndNewElement([FromBody] SecondaryAutoRef analysis, [FromQuery]string username)
        {
            var result = await _context.SecondaryAutoRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);
            try
            {
                if (result != null)
                {
                    result.Code = analysis.Code;
                    result.CarBrand = analysis.CarBrand;
                    result.CarModel = analysis.CarModel;
                    result.ProduceYear = analysis.ProduceYear;
                    result.MarketCost = analysis.MarketCost;
                    result.MaxPercentageDeviation = analysis.MaxPercentageDeviation;
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
                        CarBrand = analysis.CarBrand,
                        CarModel = analysis.CarModel,
                        ProduceYear = analysis.ProduceYear,
                        MarketCost = analysis.MarketCost,
                        MaxPercentageDeviation = analysis.MaxPercentageDeviation,
                        BeginDate = analysis.BeginDate,
                        EndDate = analysis.EndDate,
                        Action = "Добавление",
                        Username = username,
                        ChangeDate = DateTime.Today,
                        IsArch = '0',
                        TypeCode = '3',
                        Type = "Авто Вторичка",
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