using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyGetter.Helper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FlyGetter.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IConfiguration _configuration;

        public ValuesController(ILogger<LogController> logger, IConfiguration configuration)
        {

        }

        [HttpGet("Database")]
        public string DatabaseList()
        {
            JObject obj = new JObject();

            var result = DbHelper.Test(out List<string> dataList, out string msg);
            obj["msg"] = msg;
            obj["items"] = JToken.FromObject(dataList);
            obj["result"] = result;

            return obj + "";
        }
    }
}
