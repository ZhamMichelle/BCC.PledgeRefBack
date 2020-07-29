using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bcc.Pledg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Bcc.Pledg.Controllers
{

    [Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class TemporaryController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TemporaryController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("city")]
        public async Task<ActionResult> GetCity(string city)
        {
            var allList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
            return Ok(allList);
        }

        [HttpGet("search/sector")]
        public async Task<ActionResult> GetBySearchSector(string city, int? sector)
        {
            if (sector != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch(string city, int? sector, string estate)
        {
            if (sector != null && estate != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector && r.TypeEstateByRef.ToLower().Contains(estate.ToLower())).ToListAsync();
                return Ok(searchList);
            }
            else if (sector == null && estate != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.TypeEstateByRef.ToLower().Contains(estate.ToLower())).ToListAsync();
                return Ok(searchList);
            }
           else if (sector != null && estate == null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city && r.Sector == sector).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveId(int id, [FromQuery]string username)
        {

            var deleteParams = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteParams == null)
                return NotFound();
            _context.PledgeRefs.Remove(deleteParams);

            var logdata = _context.LogData.Add(new LogData()
            {
                CityCodeKATO = deleteParams.CityCodeKATO,
                City = deleteParams.City,
                SectorCode = deleteParams.SectorCode,
                Sector = deleteParams.Sector,
                RelativityLocation = deleteParams.RelativityLocation,
                SectorDescription = deleteParams.SectorDescription,
                TypeEstateCode = deleteParams.TypeEstateCode,
                TypeEstateByRef = deleteParams.TypeEstateByRef,
                ApartmentLayoutCode = deleteParams.ApartmentLayoutCode,
                ApartmentLayout = deleteParams.ApartmentLayout,
                WallMaterialCode = deleteParams.WallMaterialCode,
                WallMaterial = deleteParams.WallMaterial,
                DetailAreaCode = deleteParams.DetailAreaCode,
                DetailArea = deleteParams.DetailArea,
                MinCostPerSQM = deleteParams.MinCostPerSQM,
                MaxCostPerSQM = deleteParams.MaxCostPerSQM,
                Bargain = deleteParams.Bargain,
                MinCostWithBargain = deleteParams.MinCostWithBargain,
                MaxCostWithBargain = deleteParams.MaxCostWithBargain,
                BeginDate = deleteParams.BeginDate,
                EndDate = deleteParams.EndDate,
                Action = "Удаление",
                Username = username,
                PreviousId = id,
                ChangeDate = DateTime.Today,
            });

            await _context.SaveChangesAsync();
            return Ok("Removed");
        }

        [HttpDelete("city/{city}")]
        public async Task<ActionResult> RemoveCity(string city, [FromQuery]string username)
        {

            var deleteParams = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
            if (deleteParams == null)
                return NotFound();
            _context.PledgeRefs.RemoveRange(deleteParams);
            foreach (var item in deleteParams)
            {
                var logdata = _context.LogData.Add(new LogData()
                {
                    CityCodeKATO = item.CityCodeKATO,
                    City = item.City,
                    SectorCode = item.SectorCode,
                    Sector = item.Sector,
                    RelativityLocation = item.RelativityLocation,
                    SectorDescription = item.SectorDescription,
                    TypeEstateCode = item.TypeEstateCode,
                    TypeEstateByRef = item.TypeEstateByRef,
                    ApartmentLayoutCode = item.ApartmentLayoutCode,
                    ApartmentLayout = item.ApartmentLayout,
                    WallMaterialCode = item.WallMaterialCode,
                    WallMaterial = item.WallMaterial,
                    DetailAreaCode = item.DetailAreaCode,
                    DetailArea = item.DetailArea,
                    MinCostPerSQM = item.MinCostPerSQM,
                    MaxCostPerSQM = item.MaxCostPerSQM,
                    Bargain = item.Bargain,
                    MinCostWithBargain = item.MinCostWithBargain,
                    MaxCostWithBargain = item.MaxCostWithBargain,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    Action = "Удаление",
                    Username = username,
                    PreviousId = item.Id,
                    ChangeDate = DateTime.Today,
                });

                await _context.SaveChangesAsync();
            }
            return Ok("Removed");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id)
        {
            var getElement = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (getElement == null)
                return NotFound();
            return Ok(getElement);
        }

        [HttpPost]
        public async Task<ActionResult> AddElement([FromBody]PledgeReference analysis, [FromQuery]string username)
        {
            _context.PledgeRefs.Add(analysis);

            var logdata = _context.LogData.Add(new LogData()
            {
                CityCodeKATO = analysis.CityCodeKATO,
                City = analysis.City,
                SectorCode = analysis.SectorCode,
                Sector = analysis.Sector,
                RelativityLocation = analysis.RelativityLocation,
                SectorDescription = analysis.SectorDescription,
                TypeEstateCode = analysis.TypeEstateCode,
                TypeEstateByRef = analysis.TypeEstateByRef,
                ApartmentLayoutCode = analysis.ApartmentLayoutCode,
                ApartmentLayout = analysis.ApartmentLayout,
                WallMaterialCode = analysis.WallMaterialCode,
                WallMaterial = analysis.WallMaterial,
                DetailAreaCode = analysis.DetailAreaCode,
                DetailArea = analysis.DetailArea,
                MinCostPerSQM = analysis.MinCostPerSQM,
                MaxCostPerSQM = analysis.MaxCostPerSQM,
                Bargain = analysis.Bargain,
                MinCostWithBargain = analysis.MinCostWithBargain,
                MaxCostWithBargain = analysis.MaxCostWithBargain,
                BeginDate = analysis.BeginDate,
                EndDate = analysis.EndDate,
                Action = "Добавление",
                Username = username,
                PreviousId = _context.PledgeRefs.FirstOrDefault(p => p.Id == _context.PledgeRefs.Max(x => x.Id)).Id + 1,
                ChangeDate = DateTime.Today,
            });

            await _context.SaveChangesAsync();

            return Ok("Added");
        }

        [HttpPut]
        public async Task<ActionResult> PutElement([FromBody] PledgeReference analysis, [FromQuery]string username)
        {
            var result = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == analysis.Id);

            if (result != null)
            {

                result.CityCodeKATO = analysis.CityCodeKATO;
                result.City = analysis.City;
                result.SectorCode = analysis.SectorCode;
                result.Sector = analysis.Sector;
                result.RelativityLocation = analysis.RelativityLocation;
                result.SectorDescription = analysis.SectorDescription;
                result.TypeEstateCode = analysis.TypeEstateCode;
                result.TypeEstateByRef = analysis.TypeEstateByRef;
                result.ApartmentLayoutCode = analysis.ApartmentLayoutCode;
                result.ApartmentLayout = analysis.ApartmentLayout;
                result.WallMaterialCode = analysis.WallMaterialCode;
                result.WallMaterial = analysis.WallMaterial;
                result.DetailAreaCode = analysis.DetailAreaCode;
                result.DetailArea = analysis.DetailArea;
                result.MinCostPerSQM = analysis.MinCostPerSQM;
                result.MaxCostPerSQM = analysis.MaxCostPerSQM;
                result.Bargain = analysis.Bargain;
                result.MinCostWithBargain = analysis.MinCostWithBargain;
                result.MaxCostWithBargain = analysis.MaxCostWithBargain;
                result.BeginDate = analysis.BeginDate;
                result.EndDate = analysis.EndDate;
                

                var logdata = _context.LogData.Add(new LogData()
                {
                    CityCodeKATO = analysis.CityCodeKATO,
                    City = analysis.City,
                    SectorCode = analysis.SectorCode,
                    Sector = analysis.Sector,
                    RelativityLocation = analysis.RelativityLocation,
                    SectorDescription = analysis.SectorDescription,
                    TypeEstateCode = analysis.TypeEstateCode,
                    TypeEstateByRef = analysis.TypeEstateByRef,
                    ApartmentLayoutCode = analysis.ApartmentLayoutCode,
                    ApartmentLayout = analysis.ApartmentLayout,
                    WallMaterialCode = analysis.WallMaterialCode,
                    WallMaterial = analysis.WallMaterial,
                    DetailAreaCode = analysis.DetailAreaCode,
                    DetailArea = analysis.DetailArea,
                    MinCostPerSQM = analysis.MinCostPerSQM,
                    MaxCostPerSQM = analysis.MaxCostPerSQM,
                    Bargain = analysis.Bargain,
                    MinCostWithBargain = analysis.MinCostWithBargain,
                    MaxCostWithBargain = analysis.MaxCostWithBargain,
                    BeginDate = analysis.BeginDate,
                    EndDate = analysis.EndDate,
                    Action = "Редактирование",
                    Username = username,
                    PreviousId = analysis.Id,
                    ChangeDate = DateTime.Today,
                });
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
