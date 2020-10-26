using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public FileController(ILogger<LogController> logger, IConfiguration configuration, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClient;
        }

        [HttpPost("txt")]
        public string ExportFile([FromBody] FileExport fileExport)
        {
            _logger.LogInformation("Export file");
            var ret = new JObject();

            string filePath = Path.Combine(Environment.CurrentDirectory, $"wwwroot/exportfiles/");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmssffff}.txt";
            string fileFullName = Path.Combine(filePath, fileName);
            var sb = new StringBuilder();
            try
            {
                foreach (var column in fileExport.lable)
                {
                    sb.Append(column).Append('\t');
                }
                sb.AppendLine();
                sb.AppendLine("---------------------------------------------");

                foreach (var data in fileExport.data)
                {
                    sb.Append(data.clock).Append('\t').Append(data.errorObject).Append('\t').Append(data.desc).Append('\t').Append(data.severity).Append('\t');
                    sb.AppendLine();
                }
                System.IO.File.WriteAllText(fileFullName, sb.ToString(), Encoding.UTF8);
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

        [HttpPost("excel")]
        public string ExportExcelFile([FromBody] FileExport fileExport)
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
                    ExcelWorksheet worksheet = ep.Workbook.Worksheets.Add("export");

                    for (int i = 1; i <= fileExport.lable.Count; i++)
                    {
                        worksheet.Cells[1, i].Value = fileExport.lable[i - 1];
                    }

                    for (int i = 1; i <= fileExport.data.Count; i++)
                    {
                        worksheet.Cells[i + 1, 1].Value = fileExport.data[i - 1].clock;
                        worksheet.Cells[i + 1, 2].Value = fileExport.data[i - 1].errorObject;
                        worksheet.Cells[i + 1, 3].Value = fileExport.data[i - 1].desc;
                        worksheet.Cells[i + 1, 4].Value = fileExport.data[i - 1].severity;
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
