using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FlyGetter.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [ApiController]
    [Route("api/log")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IConfiguration _configuration;
        private int count;

        public LogController(ILogger<LogController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            count = int.Parse(_configuration["Count"]);
        }

        [HttpGet]
        public string GetLogByFileName([FromQuery] string fileName)
        {
            var obj = new JObject();
            List<string> roseLogs = new List<string>();
            string logPath = _configuration["LogPath"];
            string path = Path.Combine(logPath, fileName);
            if (!System.IO.File.Exists(path))
            {
                obj["result"] = false;
                obj["msg"] = $"日志路径：{path}，未找到日志文件！";
                obj["items"] = new JArray();
                return obj + "";
            };
            var logs = System.IO.File.ReadAllLines(path, Encoding.GetEncoding(0)).ToList();
            if (count > 0)
            {
                for (int i = logs.Count - 1; i > logs.Count - 1 - count; i--)
                {
                    roseLogs.Add(logs[i]);
                    if (i == 0) break;
                }
            }

            obj["result"] = true;
            obj["msg"] = "";
            obj["items"] = JToken.FromObject(roseLogs);
            return obj + "";
        }
    }
}
