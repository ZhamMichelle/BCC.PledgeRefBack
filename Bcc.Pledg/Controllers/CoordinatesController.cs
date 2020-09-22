using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcc.Pledg.Models;
using Bcc.Pledg.Models.Branch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.IO;
using System.Text;
using OfficeOpenXml;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bcc.Pledg.Controllers
{
    //[Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private readonly ILogger<CoordinatesController> _logger;
        private readonly SectorsCity[] _reference;
        private readonly TestClass[] _testClasses;
        public CoordinatesController(ILogger<CoordinatesController> logger)
        {
            _reference = ReferenceContext.GetReference<SectorsCity>();
            _testClasses = ReferenceContext.GetReference<TestClass>();
            _logger = logger;
        }
        [HttpPost("{reference}")]
        public string PostSector([FromForm]IFormFile body,  [FromQuery]string username) {
            var stream = body.OpenReadStream();
            try
            {
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(stream))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    string json = System.IO.File.ReadAllText($"../Bcc.Pledg/Resources/Test.json");
                    dynamic jsonObjTest = JsonConvert.DeserializeObject(json);

                    dynamic jsonObj = JsonConvert.DeserializeObject(json);
                    jsonObj[0]["city"] = worksheet.Cells[2, 1].Value != null ? worksheet.Cells[2, 1].Value.ToString() : null;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        jsonObj[0]["sectors"][row - 2]["sectorCode"] = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : null;
                        jsonObj[0]["sectors"][row-2]["sector"] = worksheet.Cells[row,3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : null;

                        //jsonObj[0]["sectors"][row - 2]["coordinates"][0] = Newtonsoft.Json.JsonConvert.DeserializeObject<Coordinates>(worksheet.Cells[row, 4].Value);
                        var jsonString = @"{""lat"":1,""lng"":2},{""lat"":3,""lng"":4}";
                        var jsonT = "[{\"lat\":57.5,\"lng\":57.5},\r\n{\"lat\":56,\"lng\":56},\r\n{\"lat\":54,\"lng\":54}]";
                        //var jsonReader = new JsonTextReader(new StringReader(jsonT))
                        //{
                        //    SupportMultipleContent = true // This is important!
                        //};
                        //var jsonSerializer = new JsonSerializer();
                        

                        List<Coordinates> test = JsonConvert.DeserializeObject<List<Coordinates>>(jsonT);
                        SectorsCity newTest = new SectorsCity {                  };
                        for (int j = 0; j < test.Count(); j++) {
                            jsonObj[0]["sectors"][row - 2]["coordinates"][j]["lat"] = test[j].lat;
                            jsonObj[0]["sectors"][row - 2]["coordinates"][j]["lng"] = test[j].lng;
                        }
                        //jsonObj[0]["sectors"][row - 2]["coordinates"] = test;
                        jsonObjTest.Add(jsonObj[0]);


                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjTest, Newtonsoft.Json.Formatting.Indented);
                        System.IO.File.WriteAllTextAsync("../Bcc.Pledg/Resources/Test.json", output);

                        string json1 = System.IO.File.ReadAllText("../Bcc.Pledg/Resources/Test.json");

                        //var logData = new LogData
                        //{
                        //    Code = "Координаты",
                        //    Action = "Excel",
                        //    Username = username,
                        //    ChangeDate = DateTime.Today,
                        //    IsArch = '0',
                        //};
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