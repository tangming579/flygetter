using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlyGetter.Model
{
    public class ProblemModel
    {
        public List<ProblemData> items { get; set; }
    }
    public class ProblemData
    {
        [Ignore]
        public int Severity
        {
            get
            {
                switch (SeverityStr)
                {
                    case "High": return 4;
                    case "Average": return 2;
                    default:
                        return 2;
                }
            }
        }
        [Name("Severity")]
        [JsonIgnore]
        public string SeverityStr { get; set; }
        public DateTime? Time { get; set; }
        [Name("Recovery time")]
        public DateTime? RecoveryTime { get; set; }
        [Name("Status")]
        [BooleanTrueValues("RESOLVED")]
        [BooleanFalseValues("PROBLEM")]
        public bool Resolved { get; set; }
        public string Host { get; set; }
        public string Problem { get; set; }
        [Name("Operational data")]
        public string OperationalData { get; set; }
    }
}
