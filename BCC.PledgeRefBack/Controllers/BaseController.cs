using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCC.PledgeRefBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BCC.PledgeRefBack.Controllers
{

    [Route("api/[controller]")]
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
         
        //    //var genericArgument = typeof(T).GetGenericArguments().FirstOrDefault();
        //    //Type t = typeof(ApplicationEntity);
        //    //return _context.Set<T>().FirstOrDefault(r => r.Id == id);
            
        //}


        [HttpPost]
        public void Post([FromBody]T value)
        {
            _context.Set<T>().Add(value);
            _context.SaveChanges();
        }
    }
    public class Tester<T> where T : class, IApplicationEntity
    {
        PostgresContext context;
        public Tester() { 
       
        }
        public ActionResult<T> test(int id)
        {
            return context.Set<T>().FirstOrDefault(r => r.Id == id);
        }
    }
}