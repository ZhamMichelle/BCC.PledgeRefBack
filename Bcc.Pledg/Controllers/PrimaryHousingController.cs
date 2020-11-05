using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bcc.Pledg.Controllers
{
    //[Authorize(Policy = "DMOD")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryHousingController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<PrimaryHousingController> _logger;

        public PrimaryHousingController(PostgresContext context, ILogger<PrimaryHousingController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("city")]
        public async Task<ActionResult> GetCity(string city)
        {
            var allList = await _context.PrimaryPledgeRefs.Where(r => r.City == city).ToListAsync();
            return Ok(allList);
        }
    }
}