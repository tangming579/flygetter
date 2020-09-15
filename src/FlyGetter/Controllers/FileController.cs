using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        [HttpGet("{json}")]
        [HttpPost("{json}")]
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

        [HttpGet("excel")]
        [HttpPost("excel")]
        public string ExportExcelFile()
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
                    worksheet.Cells[1, 1].Value = "名称";
                    worksheet.Cells[1, 2].Value = "价格";
                    worksheet.Cells[1, 3].Value = "销量";

                    worksheet.Cells[2, 1].Value = "大米";
                    worksheet.Cells[2, 2].Value = 56;
                    worksheet.Cells[2, 3].Value = 100;

                    worksheet.Cells[3, 1].Value = "玉米";
                    worksheet.Cells[3, 2].Value = 45;
                    worksheet.Cells[3, 3].Value = 150;

                    worksheet.Cells[4, 1].Value = "小米";
                    worksheet.Cells[4, 2].Value = 38;
                    worksheet.Cells[4, 3].Value = 130;

                    worksheet.Cells[5, 1].Value = "糯米";
                    worksheet.Cells[5, 2].Value = 22;
                    worksheet.Cells[5, 3].Value = 200;

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
