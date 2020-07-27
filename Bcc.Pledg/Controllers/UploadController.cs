using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using ExcelWorksheet = OfficeOpenXml.ExcelWorksheet;
using Bcc.Pledg.Models;
using Microsoft.Extensions.Logging;

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

                    for (int row = 1; row <= rowCount; row++)
                    {
                        var data = new PledgeReference
                        {
                            CityCodeKATO = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : null,
                            City = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : null,
                            SectorCode = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : null,
                            Sector = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                            RelativityLocation = worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString() : null,
                            SectorDescription = worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value.ToString() : null,
                            TypeEstateCode = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value.ToString() : null,
                            TypeEstateByRef = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString() : null,
                            TypeEstate = worksheet.Cells[row, 9].Value != null ? worksheet.Cells[row, 9].Value.ToString() : null,
                            ApartmentLayoutCode = worksheet.Cells[row, 10].Value != null ? worksheet.Cells[row, 10].Value.ToString() : null,
                            ApartmentLayout = worksheet.Cells[row, 11].Value != null ? worksheet.Cells[row, 11].Value.ToString() : null,
                            WallMaterialCode = Convert.ToInt32(worksheet.Cells[row, 12].Value),
                            WallMaterial = worksheet.Cells[row, 13].Value != null ? worksheet.Cells[row, 13].Value.ToString() : null,
                            DetailAreaCode = worksheet.Cells[row, 14].Value != null ? worksheet.Cells[row, 14].Value.ToString() : null,
                            DetailArea = worksheet.Cells[row, 15].Value != null ? worksheet.Cells[row, 15].Value.ToString() : null,
                            MinCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 16].Value),
                            MaxCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 17].Value),
                            Corridor = Convert.ToDecimal(worksheet.Cells[row, 18].Value),
                            MinCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 19].Value),
                            MaxCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 20].Value),
                            BeginDate = worksheet.Cells[row, 21].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 21].Value) : (DateTime?)null,
                            EndDate = worksheet.Cells[row, 22].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 22].Value) : (DateTime?)null
                        };
                        _context.PledgeRefs.Add(data);


                        var logData = new LogData
                        {
                            CityCodeKATO = worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString() : null,
                            City = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString() : null,
                            SectorCode = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : null,
                            Sector = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                            RelativityLocation = worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString() : null,
                            SectorDescription = worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value.ToString() : null,
                            TypeEstateCode = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value.ToString() : null,
                            TypeEstateByRef = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString() : null,
                            TypeEstate = worksheet.Cells[row, 9].Value != null ? worksheet.Cells[row, 9].Value.ToString() : null,
                            ApartmentLayoutCode = worksheet.Cells[row, 10].Value != null ? worksheet.Cells[row, 10].Value.ToString() : null,
                            ApartmentLayout = worksheet.Cells[row, 11].Value != null ? worksheet.Cells[row, 11].Value.ToString() : null,
                            WallMaterialCode = Convert.ToInt32(worksheet.Cells[row, 12].Value),
                            WallMaterial = worksheet.Cells[row, 13].Value != null ? worksheet.Cells[row, 13].Value.ToString() : null,
                            DetailAreaCode = worksheet.Cells[row, 14].Value != null ? worksheet.Cells[row, 14].Value.ToString() : null,
                            DetailArea = worksheet.Cells[row, 15].Value != null ? worksheet.Cells[row, 15].Value.ToString() : null,
                            MinCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 16].Value),
                            MaxCostPerSQM = Convert.ToInt32(worksheet.Cells[row, 17].Value),
                            Corridor = Convert.ToDecimal(worksheet.Cells[row, 18].Value),
                            MinCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 19].Value),
                            MaxCostWithBargain = Convert.ToInt32(worksheet.Cells[row, 20].Value),
                            BeginDate = worksheet.Cells[row, 21].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 21].Value) : (DateTime?)null,
                            EndDate = worksheet.Cells[row, 22].Value != null ? Convert.ToDateTime(worksheet.Cells[row, 22].Value) : (DateTime?)null,
                            Action = "Заливка через excel",
                            Username = username,
                            PreviousId = _context.PledgeRefs.FirstOrDefault(p => p.Id == _context.PledgeRefs.Max(x => x.Id)).Id != null ?
                            _context.PledgeRefs.FirstOrDefault(p => p.Id == _context.PledgeRefs.Max(x => x.Id)).Id+ 1 : 1,
                            ChangeDate = DateTime.Today,
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