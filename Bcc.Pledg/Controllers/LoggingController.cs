using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Bcc.Pledg.Controllers
{
    [Authorize(Policy = "DMOD")]
    [Route("[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly PostgresContext _context;

        public LoggingController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetCity([FromQuery]string action)
        {
            var allList = await _context.LogData.Where(r => r.Action == action).ToListAsync();
            return Ok(allList);
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetBySearch(string actionType, int? code)
        {

            if (code == null)
            {
                var searchList = await _context.LogData.Where(r => r.Action == actionType).ToListAsync();
                return Ok(searchList);
            }
            else if (code != null && actionType == "Выберите действие")
            {
                var searchList = await _context.LogData.Where(r => r.PreviousId == code).ToListAsync();
                return Ok(searchList);
            }
            else
            {
                var searchList = await _context.LogData.Where(r => r.Action == actionType && r.PreviousId == code).ToListAsync();
                return Ok(searchList);
            }
        }
    }
}