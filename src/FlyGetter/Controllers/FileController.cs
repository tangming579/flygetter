using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FlyGetter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace FlyGetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IConfiguration _configuration;

        public FileController(ILogger<LogController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("/txt/{json}")]
        [HttpPost("/txt/{json}")]
        public string ExportFile(string json)
        {
            _logger.LogInformation("Export file");
            var obj = JObject.Parse(json);
            var ret = new JObject();

            string filePath = Path.Combine(Environment.CurrentDirectory, $"wwwroot/exportfiles/");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmssffff}.txt";
            string fileFullName = Path.Combine(filePath, fileName);
            var sb = new StringBuilder();
            var columns = obj["columns"] as JArray;
            var datas = obj["datas"] as JArray;
            try
            {
                foreach (var column in columns)
                {
                    sb.Append(column).Append('\t');
                }
                sb.AppendLine();
                sb.AppendLine("--------------------------------------------------------");

                foreach (var data in datas)
                {
                    sb.Append(data["name"]).Append('\t').Append(data["time"]).Append('\t').Append(data["description"]).Append('\t');
                    sb.AppendLine();
                }
                System.IO.File.WriteAllText(fileFullName, sb.ToString(),Encoding.UTF8);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Export Error", ex);
            }
            var retData = new JObject();
            retData["url"] = $"/exportfiles/{fileName}";
            retData["localpath"] = fileFullName;
            ret["data"] = retData;
            ret["success"] = true;
            return ret + "";
        }

        [HttpGet("excel/{json}")]
        [HttpPost("excel/{json}")]
        public string ExportExcelFile(FileExport fileExport)
        {
            _logger.LogInformation("Export file");
            var obj = new JObject();

            string filePath = Path.Combine(Environment.CurrentDirectory, $"wwwroot/exportfiles/");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmssffff}.xlsx";
            string fileFullName = Path.Combine(filePath, fileName);
            FileInfo file = new FileInfo(fileFullName);
            try
            {
                using (ExcelPackage ep = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = ep.Workbook.Worksheets.Add("log");

                    for (int i = 1; i <= fileExport.datas.Count; i++)
                    {
                        worksheet.Cells[1, i].Value = fileExport.datas[i - 1].name;

                        for (int j = 1; j <= fileExport.datas[i - 1].items.Count; j++)
                        {
                            worksheet.Cells[i, j].Value = fileExport.datas[i - 1].items[j - 1];
                        }
                    }
                    ep.Save();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Export Error", ex);
            }

            var data = new JObject();
            data["url"] = $"/exportfiles/{fileName}";
            data["localpath"] = fileFullName;
            obj["data"] = data;
            obj["success"] = true;
            return obj + "";
        }
    }
}
