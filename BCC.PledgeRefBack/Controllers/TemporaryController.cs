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

        [HttpPost]
        public async Task<ActionResult> GetCity([FromBody] City city)
        {

            var allList = await _context.PledgeRefs.Where(r => r.City == city.city).ToListAsync();
            return Ok(allList);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveId([FromBody] DeleteParams delParams)
        {

            var deleteParams = await _context.PledgeRefs.Where(r => r.City== delParams.City && r.Sector==delParams.Sector).ToListAsync();
            if (deleteParams == null)
                return Ok("Таких данных нет в списке");
            _context.PledgeRefs.RemoveRange(deleteParams);
            await _context.SaveChangesAsync();
            return Ok("Удалено");
        }
    }
}