using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApi.Startup;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class testController : ControllerBase
    {

        IFreeSql _fsql { get; set; }
        private readonly ILogger<testController> _logger;

        public testController(ILogger<testController> logger, IFreeSql fsql)
        {
            _logger = logger;
            _fsql = fsql;
        }

        [HttpGet]
        public IEnumerable<Item> Get()
        {
            var ItemList = _fsql.Select<Item>().ToList();

            return ItemList;
        }

        [HttpPut]
        public IEnumerable<Item> Put(string name)
        {
            var id = _fsql.Insert<Item>().AppendData(new Item() { Text =name}).ExecuteIdentity();
            var ItemList = _fsql.Select<Item>().Where (a=>a.Id==id).ToList();
            return ItemList;
        }
    }
}
