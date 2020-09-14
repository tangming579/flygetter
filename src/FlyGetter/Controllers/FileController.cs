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

            string filePath = Path.Combine(Environment.CurrentDirectory, $"wwwroot\\exportfiles\\");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string FileName = Path.Combine(filePath, $"{DateTime.Now:yyyyMMddHHmmssffff}.txt");
            var sb = new StringBuilder();
            try
            {
                sb.Append("Name").Append('\t').Append("Age").Append('\t').Append("Value").Append('\t');
                sb.AppendLine();
                sb.AppendLine("--------------------------------------------------------");

                sb.Append("Tom").Append('\t').Append("12").Append('\t').Append("100").Append('\t');
                sb.AppendLine();


                sb.Append("Nick").Append('\t').Append("2333").Append('\t').Append("1111").Append('\t');
                sb.AppendLine();

                System.IO.File.WriteAllText(FileName, sb.ToString());
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Export Error", ex);
            }
            return FileName;
        }

        [HttpGet("excel")]
        [HttpPost("excel")]
        public string ExportExcelFile()
        {
            _logger.LogInformation("Export file");

            string filePath = Path.Combine(Environment.CurrentDirectory, $"wwwroot\\exportfiles\\");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string FileName = Path.Combine(filePath, $"{DateTime.Now:yyyyMMddHHmmssffff}.xlsx");
            FileInfo file = new FileInfo(FileName);
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
            return FileName;
        }
    }
}
