using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Bcc.Pledg.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using OfficeOpenXml;
using System.Collections.Generic;

namespace Bcc.Pledg.Controllers
{
    [Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly PostgresContext _context;

        public LoggingController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("existCities")]
        public async Task<List<string>> GetExistCities()
        {

            var arr = await _context.LogData.Select(x => new { x.City }).Distinct().ToListAsync();
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

        [HttpGet]
        public async Task<ActionResult> GetCity([FromQuery]string action)
        {
            var allList = await _context.LogData.Where(r => r.Action == action).ToListAsync();
            return Ok(allList);
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch(string actionType, string code, char? status)
        {

            if (code == null && status == null)
            {
                var searchList = await _context.LogData.Where(r => r.Action == actionType).ToListAsync();
                return Ok(searchList);
            }
            else if (code != null && actionType == "Выберите действие" && status == null)
            {
                var searchList = await _context.LogData.Where(r => r.Code == code).ToListAsync();
                return Ok(searchList);
            }
            else if (code != null && actionType != null && status != null) {
                var searchList = await _context.LogData.Where(r => r.Action == actionType && r.Code == code && r.IsArch==status).ToListAsync();
                return Ok(searchList);
            }
            else if (code == null && actionType != null && status != null)
            {
                var searchList = await _context.LogData.Where(r => r.Action == actionType  && r.IsArch == status).ToListAsync();
                return Ok(searchList);
            }
            else if (code == null && actionType == null && status != null)
            {
                var searchList = await _context.LogData.Where(r => r.IsArch == status).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.LogData.Where(r => r.Action == actionType && r.Code == code).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpGet("{page}/{size}")]
        public async Task<ActionResult<PagedResult<LogData>>> GetData(int page, int size, char? status, string code, string city, string type)
        {
            if (status != null && code == null && city==null && type==null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code != null && city==null && type == null)
            {
                var result = await _context.LogData.Where(r => r.Code == code).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code == null && city != null && type == null)
            {
                var result = await _context.LogData.Where(r => r.City == city).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code == null && city == null && type != null)
            {
                var result = await _context.LogData.Where(r => r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code != null && city == null && type == null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.Code == code).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code == null && city != null && type == null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.City == city).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code == null && city == null && type != null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status== null && code != null && city != null && type == null)
            {
                var result = await _context.LogData.Where(r => r.Code == code && r.City == city).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code != null && city == null && type != null)
            {
                var result = await _context.LogData.Where(r => r.Code == code && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code == null && city != null && type != null)
            {
                var result = await _context.LogData.Where(r => r.City == city && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code != null && city != null && type == null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.Code == code && r.City == city ).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status == null && code != null && city != null && type != null)
            {
                var result = await _context.LogData.Where(r => r.Code == code && r.City == city && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code == null && city != null && type != null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status  && r.City == city && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code != null && type != null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.Code == code && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else if (status != null && code != null && city != null && type != null)
            {
                var result = await _context.LogData.Where(r => r.IsArch == status && r.Code == code && r.City == city && r.Type == type).GetPagedAsync(page, size);
                return Ok(result);
            }
            else {
                var result = await _context.LogData.GetPagedAsync(page, size);
                return Ok(result);
            }
            
        }

        [HttpGet("download")]
        public ActionResult Download()
        {
            var logData =  _context.LogData.ToList();
            
            ExcelPackage package;
            using (package = new ExcelPackage()) {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("History");


                worksheet.Cells["A1"].Value = "Id";
                worksheet.Cells["B1"].Value = "Код строки";
                worksheet.Cells["C1"].Value = "Код города КАТО";
                worksheet.Cells["D1"].Value = "Город";
                worksheet.Cells["E1"].Value = "Код сектора города";
                worksheet.Cells["F1"].Value = "Сектор города";
                worksheet.Cells["G1"].Value = "Относительность расположения";
                worksheet.Cells["H1"].Value = "Описание сектора города";
                worksheet.Cells["I1"].Value = "Код Типа недвижимости";
                worksheet.Cells["J1"].Value = "Тип недвижимости по справочнику";
                worksheet.Cells["K1"].Value = "Код Планировка квартир";
                worksheet.Cells["L1"].Value = "Код Планировка квартир";
                worksheet.Cells["M1"].Value = "Код Материал стен";
                worksheet.Cells["N1"].Value = "Материал стен";
                worksheet.Cells["O1"].Value = "Код детализации площади по жилому дому";
                worksheet.Cells["P1"].Value = "Детализация площади по жилому дому";
                worksheet.Cells["Q1"].Value = "Стоимость за кв.м., минимальное значение";
                worksheet.Cells["R1"].Value = "Стоимость за кв.м. максимальное значение";
                worksheet.Cells["S1"].Value = "Торг, %";
                worksheet.Cells["T1"].Value = "Стоимость за кв.м., минимальное значение c торгом";
                worksheet.Cells["U1"].Value = "Стоимость за кв.м. максимальное значение c торгом";
                worksheet.Cells["V1"].Value = "Дата начала параметра";
                worksheet.Cells["W1"].Value = "Дата окончания параметра";
                worksheet.Cells["X1"].Value = "Действие";
                worksheet.Cells["Y1"].Value = "Исполнитель";
                worksheet.Cells["Z1"].Value = "Дата изменения";
                worksheet.Cells["AA1"].Value = "Статус Действ./Арх.";
                worksheet.Row(1).Style.Font.Bold = true;



                for (int i = 0; i < logData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = logData[i].Id;
                    worksheet.Cells[i + 2, 2].Value = logData[i].Code;
                    worksheet.Cells[i + 2, 3].Value = logData[i].CityCodeKATO;
                    worksheet.Cells[i + 2, 4].Value = logData[i].City;
                    worksheet.Cells[i + 2, 5].Value = logData[i].SectorCode;
                    worksheet.Cells[i + 2, 6].Value = logData[i].Sector;
                    worksheet.Cells[i + 2, 7].Value = logData[i].RelativityLocation;
                    worksheet.Cells[i + 2, 8].Value = logData[i].SectorDescription;
                    worksheet.Cells[i + 2, 9].Value = logData[i].TypeEstateCode;
                    worksheet.Cells[i + 2, 10].Value = logData[i].TypeEstateByRef;
                    worksheet.Cells[i + 2, 11].Value = logData[i].ApartmentLayoutCode;
                    worksheet.Cells[i + 2, 12].Value = logData[i].ApartmentLayout;
                    worksheet.Cells[i + 2, 13].Value = logData[i].WallMaterialCode;
                    worksheet.Cells[i + 2, 14].Value = logData[i].WallMaterial;
                    worksheet.Cells[i + 2, 15].Value = logData[i].DetailAreaCode;
                    worksheet.Cells[i + 2, 16].Value = logData[i].DetailArea;
                    worksheet.Cells[i + 2, 17].Value = logData[i].MinCostPerSQM;
                    worksheet.Cells[i + 2, 18].Value = logData[i].MaxCostPerSQM;
                    worksheet.Cells[i + 2, 19].Value = logData[i].Bargain;
                    worksheet.Cells[i + 2, 20].Value = logData[i].MinCostWithBargain;
                    worksheet.Cells[i + 2, 21].Value = logData[i].MaxCostWithBargain;
                    worksheet.Cells[i + 2, 22].Value = logData[i].BeginDate!=null ? logData[i].BeginDate.Value.ToShortDateString() : "";
                    worksheet.Cells[i + 2, 23].Value = logData[i].EndDate!=null ? logData[i].EndDate.Value.ToShortDateString() : "";
                    worksheet.Cells[i + 2, 24].Value = logData[i].Action;
                    worksheet.Cells[i + 2, 25].Value = logData[i].Username;
                    worksheet.Cells[i + 2, 26].Value = logData[i].ChangeDate.ToShortDateString();
                    worksheet.Cells[i + 2, 27].Value = logData[i].IsArch=='0' ? "Действующий" : "Архивный";
                }

                using (var stream = new MemoryStream())
                {

                    package.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"History_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}