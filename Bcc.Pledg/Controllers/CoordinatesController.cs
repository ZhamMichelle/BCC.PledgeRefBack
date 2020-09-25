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

namespace Bcc.Pledg.Controllers
{
    //[Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private readonly ILogger<CoordinatesController> _logger;
        private readonly SectorsCity[] _reference;
        private readonly SectorsCityTester[] _referenceTester;
        private readonly TestClass[] _testClasses;
        public CoordinatesController(ILogger<CoordinatesController> logger)
        {
            _reference = ReferenceContext.GetReference<SectorsCity>();
            _referenceTester = ReferenceContext.GetReference<SectorsCityTester>();
            _testClasses = ReferenceContext.GetReference<TestClass>();
            _logger = logger;
        }

        [HttpDelete("{city}/{typeLocCity}")]
        public string DeleteSector(string city, string typeLocCity)
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

        [HttpPost("{reference}")]
        public string PostSectors([FromForm]IFormFile body, [FromQuery]string username)
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
                    };


                    for (int row = 0; row <= rowCount - 2; row++)
                    {
                        string jsonMain = System.IO.File.ReadAllText($"../Bcc.Pledg/Resources/SectorsCityTest.json");
                        List<SectorsCity> totalSectors = JsonConvert.DeserializeObject<List<SectorsCity>>(jsonMain) as List<SectorsCity>;

                        for (int rowCheck = 2; rowCheck <= rowCount; rowCheck++) {
                            if (totalSectors.Any(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() &&
                             r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower() &&
                             r.sectors.Any(z => z.sector == Convert.ToInt32(worksheet.Cells[row + 2, 4].Value))))
                                return $@"Сектор под номером {Convert.ToInt32(worksheet.Cells[row + 2, 4].Value)}, " +
                                    $@"с типом нас. пункта {worksheet.Cells[row + 2, 1].Value.ToString().ToLower()}, " +
                                    $@"города {worksheet.Cells[row + 2, 2].Value.ToString()} уже имеетя в базе. " +
                                    $"Удалите этот сектор из файла эксель и заново загрузите файл.";
                        }

                        List<CoordinatesXY> points = JsonConvert.DeserializeObject<List<CoordinatesXY>>(worksheet.Cells[row + 2, 5].Value.ToString());


                        if (totalSectors.Any(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() && r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower())) {

                            totalSectors.Where(r => r.city == worksheet.Cells[row + 2, 2].Value.ToString() && r.type == worksheet.Cells[row + 2, 1].Value.ToString().ToLower())
                                .FirstOrDefault().sectors.Add(new Sectors
                                {
                                    sector = Convert.ToInt32(worksheet.Cells[row + 2, 4].Value),
                                    sectorCode = worksheet.Cells[row + 2, 3].Value.ToString(),
                                    coordinates = points
                                }
                            );
                        }
                        else {
                            element = new SectorsCity()
                            {
                                type = worksheet.Cells[row + 2, 1].Value.ToString().ToLower(),
                                city = worksheet.Cells[row + 2, 2].Value.ToString(),
                                sectors = new List<Sectors> { new Sectors {
                                sector = Convert.ToInt32(worksheet.Cells[row + 2, 4].Value),
                                sectorCode = worksheet.Cells[row + 2, 3].Value.ToString(),
                                coordinates = points
                            }}};
                            totalSectors.Add(element);
                        }

                        output = Newtonsoft.Json.JsonConvert.SerializeObject(totalSectors, Newtonsoft.Json.Formatting.Indented);
                        System.IO.File.WriteAllTextAsync("../Bcc.Pledg/Resources/SectorsCityTest.json", output);
                    }
          
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }


        [HttpPost("test/{reference}")]
        public ActionResult PostMethod([FromBody]SectorsCity sectors, string reference)
        {
            string json = System.IO.File.ReadAllText("../Bcc.Pledg/Resources/Test.json");
            dynamic jsonObjTest = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            //jsonObj[0]["city"] = sectors.city;
            jsonObj[0] = sectors;
            jsonObjTest.Add(jsonObj[0]);
           

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjTest, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllTextAsync("../Bcc.Pledg/Resources/Test.json", output);
            
            string json1 = System.IO.File.ReadAllText("../Bcc.Pledg/Resources/Test.json");
          

            var last = _reference.Where(r => r.city == "Актобе").FirstOrDefault();
            var test = ReferenceContext.PostReference(reference, sectors);
            return Ok(json1);
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
            return Raw(double.Parse(pointArr[0], formatter), double.Parse(pointArr[1], formatter), city, typeLocCity);
            //return Ray();
            //return new[] { Raw(Convert.ToDouble(pointArr[0].Replace(".", ",")), Convert.ToDouble(pointArr[1].Replace(".", ",")), city), Ray() };
        }

        public int IsDoublePointInsidePolygon(double lng, double lat, string city)
        {
            int i1, i2, n, N, flag = 0;

            double S, S1, S2, S3;
            var arr = _reference.Where(r => r.city == city).ToList();
            for (int j=0; j<arr[0].sectors.Count(); j++)
            {
                N = arr[0].sectors[j].coordinates.Count();
            for (n = 0; n < N; n++)
            {
                flag = 0;
                i1 = n < N - 1 ? n + 1 : 0;
                while (flag == 0)
                {
                    i2 = i1 + 1;
                    if (i2 >= N)
                        i2 = 0;
                    if (i2 == (n < N - 1 ? n + 1 : 0))
                        break;
                     Console.WriteLine($"j= {j}; n= {n}; i1= {i1}; i2= {i2}");
                    
                    S = Math.Abs(arr[0].sectors[j].coordinates[i1].lng * (arr[0].sectors[j].coordinates[i2].lat - arr[0].sectors[j].coordinates[n].lat) +
                             arr[0].sectors[j].coordinates[i2].lng * (arr[0].sectors[j].coordinates[n].lat - arr[0].sectors[j].coordinates[i1].lat) +
                             arr[0].sectors[j].coordinates[n].lng * (arr[0].sectors[j].coordinates[i1].lat - arr[0].sectors[j].coordinates[i2].lat));
                     Console.WriteLine($"S= {S}");
                    S1 = Math.Abs(arr[0].sectors[j].coordinates[i1].lng * (arr[0].sectors[j].coordinates[i2].lat - lat) +
                              arr[0].sectors[j].coordinates[i2].lng * (lat - arr[0].sectors[j].coordinates[i1].lat) +
                              lng * (arr[0].sectors[j].coordinates[i1].lat - arr[0].sectors[j].coordinates[i2].lat));
                     Console.WriteLine($"S1= {S1}");
                    S2 = Math.Abs(arr[0].sectors[j].coordinates[n].lng * (arr[0].sectors[j].coordinates[i2].lat - lat) +
                              arr[0].sectors[j].coordinates[i2].lng * (lat - arr[0].sectors[j].coordinates[n].lat) +
                              lng * (arr[0].sectors[j].coordinates[n].lat - arr[0].sectors[j].coordinates[i2].lat));
                     Console.WriteLine($"S2= {S2}");
                    S3 = Math.Abs(arr[0].sectors[j].coordinates[i1].lng * (arr[0].sectors[j].coordinates[n].lat - lat) +
                              arr[0].sectors[j].coordinates[n].lng * (lat - arr[0].sectors[j].coordinates[i1].lat) +
                              lng * (arr[0].sectors[j].coordinates[i1].lat - arr[0].sectors[j].coordinates[n].lat));
                     Console.WriteLine($"S3= {S3}");
                    if (Math.Abs(S - S1 - S2 - S3) <= 0.0001 || Math.Abs(S1 + S2 + S3 - S) <= 0.0001)
                    {
                        flag = arr[0].sectors[j].sector;
                        break;
                    }
                    i1 = i1 + 1;
                    if (i1 >= N)
                        i1 = 0;
                    break;
                }
                if (flag != 0)
                    break;
            }
                if (flag != 0)
                    break;
            }
            return flag;
        }

        public string Raw(double lng, double lat, string city, string typeLocCity) {
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

        public string Ray()
        {
            var arr = _testClasses.Where(r => r.city.Equals("Актобе")).ToList();
            double lng = 3;
            double lat = 2;

            int npol, c = 0;

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
                                //Console.WriteLine($"test[{i}].y={test[i].y}; test[{j}].y={test[j].y}; test[{j}].x={test[j].x}; test[{i}].x={test[i].x}");
                                c = ++c;
                            }
                        }
                    }
                }
                if (c % 2 != 0)
                {
                    return "IN POLYHEDRON";
                }
            }
            return "not in";
        }

    }
}