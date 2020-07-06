using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCC.PledgeRefBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : Controller where T : class
    {
        private readonly PostgresContext _context;

        public BaseController(PostgresContext context) {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            return _context.Set<T>();
        }

        //[HttpGet("{id}")]
        //public T Get(int id)
        //{
        //    return _context.Set<T>().FirstOrDefault(r=>r.);
        //}

        [HttpPost]
        public void Post([FromBody]T value)
        {
            _context.Set<T>().Add(value);
            _context.SaveChanges();
        }
    }
}