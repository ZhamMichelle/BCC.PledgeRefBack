using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using ExcelWorksheet = OfficeOpenXml.ExcelWorksheet;
using Bcc.Pledg.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Bcc.Pledg.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly PostgresContext _context;
        private readonly ILogger<UploadController> _logger;
        public UploadController(IHostingEnvironment hostingEnvironment, PostgresContext context, ILogger<UploadController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        public string Import([FromForm]IFormFile body, [FromQuery]string username)
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

                    for (int row = 1; row <= rowCount; row++) {
                        if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 2].Value == null ||
                          worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null ||
                          worksheet.Cells[row, 5].Value == null || worksheet.Cells[row, 7].Value ==null ||
                          worksheet.Cells[row, 8].Value == null || worksheet.Cells[row, 9].Value == null ||
                          worksheet.Cells[row, 16].Value==null || worksheet.Cells[row, 17].Value==null ||
                          worksheet.Cells[row, 18].Value==null || worksheet.Cells[row, 19].Value==null ||
                          worksheet.Cells[row, 20].Value==null) 
                        {
                            
                            return $"Пустое поле на строке: {row}";
                        }
                    };

                    for (int row = 1; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1].Value.GetType() != typeof(string) || 
                          worksheet.Cells[row, 3].Value.GetType() != typeof(string) || worksheet.Cells[row, 4].Value.GetType() != typeof(string) ||
                          worksheet.Cells[row, 5].Value.GetType() != typeof(double) || 
                          worksheet.Cells[row, 7].Value.GetType() != typeof(string) || worksheet.Cells[row, 8].Value.GetType() != typeof(string) ||
                          worksheet.Cells[row, 9].Value.GetType() != typeof(string) ||
                          worksheet.Cells[row, 16].Value.GetType() != typeof(double) ||
                          worksheet.Cells[row, 17].Value.GetType() != typeof(double) || worksheet.Cells[row, 18].Value.GetType() != typeof(double) ||
                          worksheet.Cells[row, 19].Value.GetType() != typeof(double) || worksheet.Cells[row, 20].Value.GetType() != typeof(double))
                        {
                            return $"Неправильный формат на строке: {row}";
                        }
                    };


                    for (int row = 1; row <= rowCount; row++)
                    {
                        var data = new PledgeReference
                        {
                            Code = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : null,  //
                            CityCodeKATO = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : null,//
                            City = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : null,//
                            SectorCode = worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString() : null,//
                            Sector = Convert.ToInt32(worksheet.Cells[row, 5].Value),//
                            RelativityLocation = worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value.ToString() : null,
                            SectorDescription = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value.ToString() : null,//
                            TypeEstateCode = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString() : null,//
                            TypeEstateByRef = worksheet.Cells[row, 9].Value != null ? worksheet.Cells[row, 9].Value.ToString() : null,//
                            ApartmentLayoutCode = worksheet.Cells[row, 10].Value != null ? worksheet.Cells[row, 10].Value.ToString() : null,
                            ApartmentLayout = worksheet.Cells[row, 11].Value != null ? worksheet.Cells[row, 11].Value.ToString() : null,
                            WallMaterialCode = Convert.ToInt32(worksheet.Cells[row, 12].Value),
                            WallMaterial = worksheet.Cells[row, 13].Value != null ? worksheet.Cells[row, 13].Value.ToString() : null,
                            DetailAreaCode = worksheet.Cells[row, 14].Value != null ? worksheet.Cells[row, 14].Value.ToString() : null,
                            DetailArea = worksheet.Cells[row, 15].Value != null ? worksheet.Cells[row, 15].Value.ToString() : null,
                            MinCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 16].Value),//
                            MaxCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 17].Value),//
                            Bargain = Convert.ToDecimal(worksheet.Cells[row, 18].Value),//
                            MinCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 19].Value),//
                            MaxCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 20].Value),//
                            BeginDate = worksheet.Cells[row, 21].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 21].Value) : (DateTime?)null,
                            EndDate = worksheet.Cells[row, 22].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 22].Value) : (DateTime?)null
                        };
                        if (_context.PledgeRefs.Any(r => r.Code == data.Code)) {
                            _context.PledgeRefs.Remove(_context.PledgeRefs.FirstOrDefault(f=>f.Code==data.Code));
                        };

                        _context.PledgeRefs.Add(data);


                        var logData = new LogData
                        {
                            Code = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : null,
                            CityCodeKATO = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : null,
                            City = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : null,
                            SectorCode = worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString() : null,
                            Sector = Convert.ToInt32(worksheet.Cells[row, 5].Value),
                            RelativityLocation = worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value.ToString() : null,
                            SectorDescription = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value.ToString() : null,
                            TypeEstateCode = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString() : null,
                            TypeEstateByRef = worksheet.Cells[row, 9].Value != null ? worksheet.Cells[row, 9].Value.ToString() : null,
                            ApartmentLayoutCode = worksheet.Cells[row, 10].Value != null ? worksheet.Cells[row, 10].Value.ToString() : null,
                            ApartmentLayout = worksheet.Cells[row, 11].Value != null ? worksheet.Cells[row, 11].Value.ToString() : null,
                            WallMaterialCode = Convert.ToInt32(worksheet.Cells[row, 12].Value),
                            WallMaterial = worksheet.Cells[row, 13].Value != null ? worksheet.Cells[row, 13].Value.ToString() : null,
                            DetailAreaCode = worksheet.Cells[row, 14].Value != null ? worksheet.Cells[row, 14].Value.ToString() : null,
                            DetailArea = worksheet.Cells[row, 15].Value != null ? worksheet.Cells[row, 15].Value.ToString() : null,
                            MinCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 16].Value),
                            MaxCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 17].Value),
                            Bargain = Convert.ToDecimal(worksheet.Cells[row, 18].Value),
                            MinCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 19].Value),
                            MaxCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 20].Value),
                            BeginDate = worksheet.Cells[row, 21].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 20].Value) : (DateTime?)null,
                            EndDate = worksheet.Cells[row, 22].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 21].Value) : (DateTime?)null,
                            Action = "Excel",
                            Username = username,
                            ChangeDate = DateTime.Today,
                            IsArch='0',
                        };

                        if (_context.LogData.Any(r => r.Code == data.Code))
                        {
                          var oldData =  _context.LogData.Where(f => f.Code == data.Code && f.EndDate==null).ToList();
                            foreach (var item in oldData)
                            {
                                item.EndDate = worksheet.Cells[row, 21].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 20].Value).AddDays(-1) : (DateTime?)null;
                                item.IsArch='1';
                            }
                        };

                        _context.LogData.Add(logData);
                        _context.SaveChanges();
                    }
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }


    }
}