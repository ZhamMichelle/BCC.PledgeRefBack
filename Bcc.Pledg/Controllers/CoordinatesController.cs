using System;
using System.Collections.Generic;
using System.Linq;
using Bcc.Pledg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using OfficeOpenXml;
using Newtonsoft.Json;
using Bcc.Pledg.Models.Branch;
using Microsoft.EntityFrameworkCore;
using Bcc.Pledg.Models.CoordinatesBD;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace Bcc.Pledg.Controllers
{
    //[Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private readonly ILogger<CoordinatesController> _logger;
        private readonly PostgresContext _context;
        private readonly SectorsCity[] _reference;
        private readonly SectorsCityTester[] _referenceTester;
        private readonly TestClass[] _testClasses;
        public CoordinatesController(ILogger<CoordinatesController> logger, PostgresContext context)
        {
            _reference = ReferenceContext.GetReference<SectorsCity>();
            _referenceTester = ReferenceContext.GetReference<SectorsCityTester>();
            _testClasses = ReferenceContext.GetReference<TestClass>();
            _logger = logger;
            _context = context;
        }

        [HttpGet("existCities")]
        public async Task<List<string>> GetExistCities() {

            var arr = await _context.SectorsCityDB.ToListAsync();
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

        [HttpDelete("json/{city}/{typeLocCity}")]
        public string DeleteSectorJson(string city, string typeLocCity)
        {
            try
            {
                string jsonMain = System.IO.File.ReadAllText($"../Bcc.Pledg/Resources/SectorsCityTest.json");

                List<SectorsCity> totalSectors = JsonConvert.DeserializeObject<List<SectorsCity>>(jsonMain) as List<SectorsCity>;
                if (totalSectors.Any(r => r.city.Equals(city) && r.type.Equals(typeLocCity.ToLower()))) {
                    var helperArr = totalSectors.Where(r => r.city.Equals(city) && r.type.Equals(typeLocCity.ToLower())).ToList();  //без хелпера Except почему то не читает. WTF
                    var arr = totalSectors.Except(helperArr).ToList();

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(arr, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllTextAsync("../Bcc.Pledg/Resources/SectorsCityTest.json", output);

                    return "Deleted";
                }
                return "NoData";
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return "Error delete";
            }
        }

        [HttpDelete("{city}/{typeLocCity}")]
        public async Task<ActionResult> DeleteSectorDB(string city, string typeLocCity)
        {
            try
            {
                var arrCity =  _context.SectorsCityDB.Include(i => i.SectorsDB).FirstOrDefault(r => r.City.Equals(city) && r.Type.Equals(typeLocCity.ToLower()));

                if (arrCity!=null)
                {
                    var arrSector = _context.SectorsDB.Include(i => i.CoordinatesDB)
                   .Where(r => r.SectorsCityDBId == arrCity.Id).ToList();

                    var arrCoordinates = _context.CoordinatesDB.AsEnumerable().Where(x => arrSector.Any(key => x.SectorsDBId.Contains(key.Id))).ToList();

                    _context.CoordinatesDB.RemoveRange(arrCoordinates);
                    _context.SectorsDB.RemoveRange(arrSector);
                    _context.SectorsCityDB.RemoveRange(arrCity);

                    await _context.SaveChangesAsync();

                    return Ok("Deleted");
                }
                return Ok("NoData");
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return Ok("Error delete");
            }
        }
        [HttpPost("json/{reference}")]
        public string PostSectorsJson([FromForm]IFormFile body, [FromQuery]string username)
        {
            var stream = body.OpenReadStream();
            try
            {
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(stream))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    var element = new SectorsCity();
                    string output;

                    //Проверка
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 2].Value == null ||
                         worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null ||
                         worksheet.Cells[row, 5].Value == null)
                        {
                            return $"Пустое поле на строке: {row}";
                        }
                        else if (worksheet.Cells[row, 1].Value.GetType() != typeof(string) || worksheet.Cells[row, 2].Value.GetType() != typeof(string) ||
                          worksheet.Cells[row, 3].Value.GetType() != typeof(string) || worksheet.Cells[row, 4].Value.GetType() != typeof(double) ||
                          worksheet.Cells[row, 5].Value.GetType() != typeof(string))
                        {
                            return $"Неправильный формат на строке: {row}";
                        }

                        try
                        {
                            JsonConvert.DeserializeObject<List<CoordinatesXY>>(worksheet.Cells[row, 5].Value.ToString());
                        }
                        catch (Exception e)
                        {
                            return $"Неправильный формат координат на строке: {row}";
                        }

                    };


                    for (int row = 0; row <= rowCount - 2; row++)
                    {
                        try
                        {
                            string jsonMain = System.IO.File.ReadAllText($"../Bcc.Pledg/Resources/SectorsCityTest.json");
                            List<SectorsCity> totalSectors = JsonConvert.DeserializeObject<List<SectorsCity>>(jsonMain) as List<SectorsCity>;

                            for (int rowCheck = 2; rowCheck <= rowCount; rowCheck++)
                            {
                                if (totalSectors.Any(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() &&
                                 r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower() &&
                                 r.sectors.Any(z => z.sector == worksheet.Cells[row, 4].Value.ToString())))
                                    return $@"Сектор под номером {Convert.ToInt32(worksheet.Cells[row + 2, 4].Value)}, " +
                                        $@"с типом нас. пункта {worksheet.Cells[row + 2, 1].Value.ToString().ToLower()}, " +
                                        $@"города {worksheet.Cells[row + 2, 2].Value.ToString()} уже имеетя в базе. " +
                                        $"Удалите этот сектор из файла эксель и заново загрузите файл.";
                            }

                            List<CoordinatesXY> points = JsonConvert.DeserializeObject<List<CoordinatesXY>>(worksheet.Cells[row + 2, 5].Value.ToString());


                            if (totalSectors.Any(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() && r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower()))
                            {

                                totalSectors.Where(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() && r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower())
                                    .FirstOrDefault().sectors.Add(new Sectors
                                    {
                                        sector = worksheet.Cells[row, 4].Value.ToString(),
                                        sectorCode = worksheet.Cells[row + 2, 3].Value.ToString(),
                                        coordinates = points
                                    }
                                );
                            }
                            else
                            {
                                element = new SectorsCity()
                                {
                                    type = worksheet.Cells[row + 2, 1].Value.ToString().ToLower(),
                                    city = worksheet.Cells[row + 2, 2].Value.ToString(),
                                    sectors = new List<Sectors> { new Sectors {
                                sector = worksheet.Cells[row, 4].Value.ToString(),
                                sectorCode = worksheet.Cells[row + 2, 3].Value.ToString(),
                                coordinates = points
                            }}
                                };
                                totalSectors.Add(element);
                            }

                            output = Newtonsoft.Json.JsonConvert.SerializeObject(totalSectors, Newtonsoft.Json.Formatting.Indented);
                            System.IO.File.WriteAllTextAsync("../Bcc.Pledg/Resources/SectorsCityTest.json", output);

                        }
                        catch (Exception exc) {
                            return $"Cтрока: {row}. Ошибка: {BadRequest(exc)}";
                        }
                    }
          
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        [HttpGet("sectors")]
        public ActionResult GetRegions()
        {
            var test = _reference.Where(r => r.city == "Актобе");
            return Ok(test);
        }

        [HttpGet("{city}/{point}/{typeLocCity}")]
        public string SearchSector(string city, string typeLocCity, string point)
        {
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            string[] pointArr = point.Split(" ");
            return RawDB(double.Parse(pointArr[0], formatter), double.Parse(pointArr[1], formatter), city, typeLocCity);
            //return new[] { Raw(Convert.ToDouble(pointArr[0].Replace(".", ",")), Convert.ToDouble(pointArr[1].Replace(".", ",")), city), Ray() };
        }

        public string RawJson(double lng, double lat, string city, string typeLocCity) {
            int npol, c = 0;
            var arr = _reference.Where(r => r.city.Equals(city) && r.type.Equals(typeLocCity)).ToList();
            //double lngTEST = 57.1887;
            //double latTEST = 50.275657;
            for (int k = 0; k < arr[0].sectors.Count(); k++)
            {
                c = 0;
                npol = arr[0].sectors[k].coordinates.Count();
                for (int i = 0, j = npol - 1; i < npol; j = i++)
                {
                    if (((arr[0].sectors[k].coordinates[i].lat <= lat) && (lat < arr[0].sectors[k].coordinates[j].lat)) || 
                        ((arr[0].sectors[k].coordinates[j].lat <= lat) && (lat < arr[0].sectors[k].coordinates[i].lat)))
                    {
                        if ((arr[0].sectors[k].coordinates[j].lat - arr[0].sectors[k].coordinates[i].lat) != 0)
                        {
                            if (lng > ((arr[0].sectors[k].coordinates[j].lng - arr[0].sectors[k].coordinates[i].lng) * (lat - arr[0].sectors[k].coordinates[i].lat) / (arr[0].sectors[k].coordinates[j].lat - arr[0].sectors[k].coordinates[i].lat) + arr[0].sectors[k].coordinates[i].lng))
                                {
                                    //Console.WriteLine($"arr[{i}].lat={arr[0].sectors[k].coordinates[i].lat}; arr[{j}].lat={arr[0].sectors[k].coordinates[j].lat}; arr[{j}].x={arr[0].sectors[k].coordinates[j].lng}; arr[{i}].x={arr[0].sectors[k].coordinates[i].lng}");
                                    c = ++c;
                                }
                        }
                    }
                }
                if (c % 2 != 0) {
                    return arr[0].sectors[k].sector.ToString();
                }
            }
            return "отсутствует";
        }

        public string RawDB(double lng, double lat, string city, string typeLocCity)
        {
            int npol, c = 0;
            List<CoordinatesDB> sortedCDB = new List<CoordinatesDB>();
            var arr = _context.SectorsDB.Include(i => i.CoordinatesDB)
                .Where(r => r.SectorsCityDBId == _context.SectorsCityDB.Include(i => i.SectorsDB).FirstOrDefault(r => r.City.Equals(city) && r.Type.Equals(typeLocCity.ToLower())).Id).ToList();

            for (int k = 0; k < arr.Count(); k++)
            {
                sortedCDB = arr[k].CoordinatesDB.OrderBy(p=>p.SortIndex).ToList();
                c = 0;
                npol = arr[k].CoordinatesDB.Count();
                for (int i = 0, j = npol - 1; i < npol; j = i++)
                {
                    if (((sortedCDB[i].Lat <= lat) && (lat < sortedCDB[j].Lat)) ||
                        ((sortedCDB[j].Lat <= lat) && (lat < sortedCDB[i].Lat)))
                    {
                        if ((sortedCDB[j].Lat - sortedCDB[i].Lat) != 0)
                        {
                            if (lng > ((sortedCDB[j].Lng - sortedCDB[i].Lng) * (lat - sortedCDB[i].Lat) / (sortedCDB[j].Lat - sortedCDB[i].Lat) + sortedCDB[i].Lng))
                            {
                                 c = ++c;
                            }
                        }
                    }
                }
                if (c % 2 != 0)
                {
                    return arr[k].Sector.ToString();
                }
            }
            return "отсутствует";
        }


        [HttpPost("upload/{reference}")]
        public async Task<ActionResult> PostSectorsToDB([FromForm]IFormFile body, [FromQuery]string username)
        {
            var stream = body.OpenReadStream();
            try
            {
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(stream))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    var element = new SectorsCityDB();
                    string output;

                    //Проверка
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 2].Value == null ||
                         worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null ||
                         worksheet.Cells[row, 5].Value == null)
                        {
                            return Ok($"Пустое поле на строке: {row}");
                        }
                        else if (worksheet.Cells[row, 1].Value.GetType() != typeof(string) || worksheet.Cells[row, 2].Value.GetType() != typeof(string) ||
                          worksheet.Cells[row, 3].Value.GetType() != typeof(string) || worksheet.Cells[row, 4].Value.GetType() != typeof(double) ||
                          worksheet.Cells[row, 5].Value.GetType() != typeof(string))
                        {
                            return Ok($"Неправильный формат на строке: {row}");
                        }

                        try
                        {
                            JsonConvert.DeserializeObject<List<CoordinatesXY>>(worksheet.Cells[row, 5].Value.ToString());
                        }
                        catch (Exception e)
                        {
                            return Ok($"Неправильный формат координат на строке: {row}");
                        }

                    };


                    for (int row = 0; row <= rowCount-2; row++)
                    {
                        try
                        {
                            var data = _context.SectorsCityDB.Include(i => i.SectorsDB).ToList();

                            for (int rowCheck = 2; rowCheck <= rowCount; rowCheck++)
                            {
                                if (data.Any(r => r.City == worksheet.Cells[row + 2, 2].Value.ToString() &&
                                 r.Type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower() &&
                                 r.SectorsDB.Any(z => z.Id == worksheet.Cells[row + 2, 3].Value.ToString())))
                                    return Ok($@"Сектор под номером {Convert.ToInt32(worksheet.Cells[row + 2, 4].Value)}, " +
                                        $@"с типом нас. пункта {worksheet.Cells[row + 2, 1].Value.ToString().ToLower()}, " +
                                        $@"города {worksheet.Cells[row + 2, 2].Value.ToString()} уже имеетя в базе. " +
                                        $"Удалите этот сектор из файла эксель и заново загрузите файл.");
                            }




                            List<CoordinatesDB> points = JsonConvert.DeserializeObject<List<CoordinatesDB>>(worksheet.Cells[row + 2, 5].Value.ToString());
                            var test = points;

                            if (data.Any(r => r.City == worksheet.Cells[row + 2, 2].Value.ToString() && r.Type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower()))
                            {

                                _context.SectorsCityDB.Where(r => r.City == worksheet.Cells[row + 2, 2].Value.ToString() && r.Type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower())
                                    .FirstOrDefault().SectorsDB.Add(
                                    new SectorsDB
                                    {
                                        Id = worksheet.Cells[row + 2, 3].Value.ToString(),
                                        Sector = worksheet.Cells[row + 2, 4].Value.ToString(),
                                        //CoordinatesDB = points,
                                    }
                                );
                                _context.CoordinatesDB.AddRange(points.Select((p, i) =>
                                {
                                    p.SectorsDBId = worksheet.Cells[row + 2, 3].Value.ToString();
                                    p.SortIndex = i;
                                    return p;
                                }));
                            }
                            else
                            {
                                element = new SectorsCityDB()
                                {
                                    Type = worksheet.Cells[row + 2, 1].Value.ToString().ToLower(),
                                    City = worksheet.Cells[row + 2, 2].Value.ToString(),
                                    SectorsDB = new List<SectorsDB> {
                                        new SectorsDB
                                        {
                                            Sector = worksheet.Cells[row+2, 4].Value.ToString(),
                                            Id = worksheet.Cells[row + 2, 3].Value.ToString(),
                                            //CoordinatesDB = points
                                        }
                                    }
                                };
                                _context.SectorsCityDB.Add(element);
                                _context.CoordinatesDB.AddRange(points.Select((p, i) =>
                                {
                                    p.SectorsDBId = worksheet.Cells[row + 2, 3].Value.ToString();
                                    p.SortIndex = i;
                                    return p;
                                }));

                            }
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception exc)
                        {
                            return Ok($"Cтрока: {row + 2}. Ошибка: {BadRequest(exc)}");
                        }
                    }

                    return Ok("Ok");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error");
            }
        }

    }
}