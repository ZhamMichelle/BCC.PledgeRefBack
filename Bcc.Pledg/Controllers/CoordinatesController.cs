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

namespace Bcc.Pledg.Controllers
{
    [Authorize(Policy = "DMOD")]
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

        [HttpGet("sectors")]
        public ActionResult GetRegions()
        {
            var test = _reference.Where(r => r.city == "Актобе");
            return Ok(test);
        }

        [HttpGet("{city}/{point}")]
        public string[] SearchSector(string city, string point )
        {

            string[] pointArr = point.Split(" ");
            //return Raw(Convert.ToDouble(pointArr[0].Replace(".", ",")), Convert.ToDouble(pointArr[1].Replace(".", ",")), city);
            //return Ray();
            return new[] { Raw(Convert.ToDouble(pointArr[0].Replace(".", ",")), Convert.ToDouble(pointArr[1].Replace(".", ",")), city), Ray() };
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

        public string Raw(double lng, double lat, string city) {
            int npol, c = 0;
            var arr = _reference.Where(r => r.city.Equals(city)).ToList();
            double lngTEST = 57.1887;
            double latTEST = 50.275657;
            for (int k = 0; k < arr[0].sectors.Count(); k++)
            {
                c = 0;
                npol = arr[0].sectors[k].coordinates.Count();
                for (int i = 0, j = npol - 1; i < npol; j = i++)
                {
                    if (((arr[0].sectors[k].coordinates[i].lat <= latTEST) && (latTEST < arr[0].sectors[k].coordinates[j].lat)) || 
                        ((arr[0].sectors[k].coordinates[j].lat <= latTEST) && (latTEST < arr[0].sectors[k].coordinates[i].lat)))
                    {
                        if ((arr[0].sectors[k].coordinates[j].lat - arr[0].sectors[k].coordinates[i].lat) != 0)
                        {
                            if (lngTEST > ((arr[0].sectors[k].coordinates[j].lng - arr[0].sectors[k].coordinates[i].lng) * (latTEST - arr[0].sectors[k].coordinates[i].lat) / (arr[0].sectors[k].coordinates[j].lat - arr[0].sectors[k].coordinates[i].lat) + arr[0].sectors[k].coordinates[i].lng))
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