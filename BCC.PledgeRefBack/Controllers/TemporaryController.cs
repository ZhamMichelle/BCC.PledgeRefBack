using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCC.PledgeRefBack.Models;
using Microsoft.EntityFrameworkCore;

namespace BCC.PledgeRefBack.Controllers
{
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

        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch(string city, int? sector) {
            if (sector != null)
            {
                var searchList = await _context.PledgeRefs.Where(r => r.City==city && r.Sector == sector).ToListAsync();
                return Ok(searchList);
            }
            else {
                var searchList = await _context.PledgeRefs.Where(r => r.City == city).ToListAsync();
                return Ok(searchList);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveId(int id)
        {

            var deleteParams = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (deleteParams == null)
                return NotFound();
            _context.PledgeRefs.Remove(deleteParams);
            await _context.SaveChangesAsync();
            return Ok("Removed");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetId(int id) {
            var getElement = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == id);
            if (getElement == null)
                return NotFound();
            return Ok(getElement);
        }

        [HttpPost]
        public async Task<ActionResult> AddElement([FromBody] PledgeReference element)
        {
            var postParam = new PledgeReference
            {
                CityCodeKATO = element.CityCodeKATO,
                City = element.City,
                SectorCode = element.SectorCode,
                Sector = element.Sector,
                RelativityLocation = element.RelativityLocation,
                SectorDescription = element.SectorDescription,
                TypeEstateCode = element.TypeEstateCode,
                TypeEstateByRef = element.TypeEstateByRef,
                TypeEstate = element.TypeEstate,
                ApartmentLayoutCode = element.ApartmentLayoutCode,
                ApartmentLayout = element.ApartmentLayout,
                WallMaterialCode = element.WallMaterialCode,
                WallMaterial = element.WallMaterial,
                DetailAreaCode = element.DetailAreaCode,
                DetailArea = element.DetailArea,
                MinCostPerSQM = element.MinCostPerSQM,
                MaxCostPerSQM = element.MaxCostPerSQM,
                Corridor = element.Corridor,
                MinCostWithBargain = element.MinCostWithBargain,
                MaxCostWithBargain = element.MaxCostWithBargain,
                BeginDate = element.BeginDate,
                EndDate = element.EndDate,
            };

            _context.PledgeRefs.Add(postParam);
            await _context.SaveChangesAsync();

            return Ok("Added");
        }

        [HttpPut]
        public async Task<ActionResult> PutElement([FromBody] PledgeReference putParam) {
            var result = await _context.PledgeRefs.FirstOrDefaultAsync(r => r.Id == putParam.Id);
            if (result != null) {
                result.CityCodeKATO = putParam.CityCodeKATO;
                result.City = putParam.City;
                result.SectorCode = putParam.SectorCode;
                result.Sector = putParam.Sector;
                result.RelativityLocation = putParam.RelativityLocation;
                result.SectorDescription = putParam.SectorDescription;
                result.TypeEstateCode = putParam.TypeEstateCode;
                result.TypeEstateByRef = putParam.TypeEstateByRef;
                result.TypeEstate = putParam.TypeEstate;
                result.ApartmentLayoutCode = putParam.ApartmentLayoutCode;
                result.ApartmentLayout = putParam.ApartmentLayout;
                result.WallMaterialCode = putParam.WallMaterialCode;
                result.WallMaterial = putParam.WallMaterial;
                result.DetailAreaCode = putParam.DetailAreaCode;
                result.DetailArea = putParam.DetailArea;
                result.MinCostPerSQM = putParam.MinCostPerSQM;
                result.MaxCostPerSQM = putParam.MaxCostPerSQM;
                result.Corridor = putParam.Corridor;
                result.MinCostWithBargain = putParam.MinCostWithBargain;
                result.MaxCostWithBargain = putParam.MaxCostWithBargain;
                result.BeginDate = putParam.BeginDate;
                result.EndDate = putParam.EndDate;
            }
            return Ok();
        }
    }
}