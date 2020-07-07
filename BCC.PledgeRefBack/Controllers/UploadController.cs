using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ExcelWorksheet = OfficeOpenXml.ExcelWorksheet;
using BCC.PledgeRefBack.Models;

namespace BCC.PledgeRefBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly PostgresContext _context;
        public UploadController(IHostingEnvironment hostingEnvironment, PostgresContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        [HttpGet]
        public string Import()
        {
            string sWebRootFolder = _hostingEnvironment.ContentRootPath;
            string sFileName = "C:/Users/User/Desktop/File/AktauPledge.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Лист1"];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                   
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var data = new PledgeReference
                        {
                            CityCodeKATO = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                            City = worksheet.Cells[row, 2].Value!=null ? worksheet.Cells[row, 2].Value.ToString() : null,
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
                            CostDescription = worksheet.Cells[row, 18].Value != null ? worksheet.Cells[row, 18].Value.ToString() : null,
                        };
                        _context.PledgeRefs.Add(data);
                        _context.SaveChanges();
                    }
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return "Some error occured while importing." + ex.Message;
            }
        }

        //[HttpPost]
        //public ActionResult Upload([FromBody]IFormFile file) {
        //    if (file == null || file.OpenReadStream() == null)
        //    {
        //        //ModelState.AddModelError("file", "Файл не выбран");
        //        return Ok();
        //    }

        //    if (System.IO.Path.GetExtension(file.FileName) != ".xlsx")
        //    {
        //       // ModelState.AddModelError("file", "Выберите файл с расширением xlsx");
        //        return Ok();
        //    }
        //    StringBuilder sbSql = new StringBuilder();

        //    var xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Worksheet)xlApp.Worksheets["СВОД"];

        //    Microsoft.Office.Interop.Excel.Range last = worksheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);


        //    for (int j = 1; j <= last.Row; j++)
        //{
        //                if (worksheet.Cells[j, 1] == null) continue;

        //                sbSql.AppendFormat(@"INSERT INTO [TableDestination] ([Column2],[Column3],[Column4]) VALUES ('{0}','{1}','{2}');", GV(worksheet, j, 2), GV(worksheet, j, 3), GV(worksheet, j, 4));
        //            }

        //    var connection = new SqlConnection("Host=localhost;Port=5432;Database=Pledge;Username=postgres;Password=fuckdas26#l");

        //        var command = new SqlCommand(sbSql.ToString(), connection);
        //        connection.Open();
        //        int result = command.ExecuteNonQuery();

        //    return Content("success");
        //}

        //private string GV(Microsoft.Office.Interop.Excel.Worksheet sheet, int rowNo, int cellNo)
        //{
        //    return sheet.Cells[rowNo, cellNo] != null ? sheet.Cells[rowNo, cellNo].ToString() : "";
        //}
    }
}