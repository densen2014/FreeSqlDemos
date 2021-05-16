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

        /// <summary>
        /// 测试api工作是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public string Test() => "OK";


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            var ItemList = _fsql.Select<Item>().ToList();

            return ItemList;
        }

        /// <summary>
        /// put或者GET的put方法插入一条数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Put")]
        [HttpPut]
        public IEnumerable<Item> Put(string name)
        {
            var id = _fsql.Insert<Item>().AppendData(new Item() { Text =name}).ExecuteIdentity();
            var ItemList = _fsql.Select<Item>().Where (a=>a.Id==id).ToList();
            return ItemList;
        }
    }
}
