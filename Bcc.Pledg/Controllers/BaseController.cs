﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcc.Pledg.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bcc.Pledg.Controllers
{
    [ApiController]
    [Route("reference/api/[controller]")]
    [Route("api/[controller]")]
    //public class BaseController<T> : Controller where T : class
    public class BaseController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<BaseController> _logger;

        public BaseController(PostgresContext context, ILogger<BaseController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{reference}")]
        public IEnumerable<object> GetJsonFile(string reference)
        {
            var result = ReferenceContext.GetReference(reference);
            return result as IEnumerable<object>;
        }

        //[HttpGet]
        //public IEnumerable<T> Get()
        //{
        //    return _context.Set<T>();
        //}

        //[HttpGet("{id}")]
        //public T Get(int id)
        //{
         
        //    //var genericArgument = typeof(T).GetGenericArguments().FirstOrDefault();
        //    //Type t = typeof(ApplicationEntity);
        //    //return _context.Set<T>().FirstOrDefault(r => r.Id == id);
            
        //}


        //[HttpPost]
        //public void Post([FromBody]T value)
        //{
        //    _context.Set<T>().Add(value);
        //    _context.SaveChanges();
        //}
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