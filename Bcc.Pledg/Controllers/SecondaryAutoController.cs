using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bcc.Pledg.Models;
using Microsoft.EntityFrameworkCore;

namespace Bcc.Pledg.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecondaryAutoController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<SecondaryAutoController> _logger;

        public SecondaryAutoController(PostgresContext context, ILogger<SecondaryAutoController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getList")]
        public async Task<List<SecondaryAutoRef>> getList() {

            var autoList = await _context.SecondaryAutoRefs.ToListAsync();

            return autoList;
        }

        [HttpGet("{id}")]
        public async Task<SecondaryAutoRef> getId(int id) {
            var autoElement = await _context.SecondaryAutoRefs.FirstOrDefaultAsync(z => z.Id == id);
            return autoElement;
        }
    }
}