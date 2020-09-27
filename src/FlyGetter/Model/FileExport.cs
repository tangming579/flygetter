using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyGetter.Model
{
    public class FileExport
    {
        public List<string> lable { get; set; }
        public List<FileData> data { get; set; }
    }

    public class FileData
    {
        public string clock { get; set; }
        public string errorObject { get; set; }
        public string desc { get; set; }
        public string severity { get; set; }
    }
}
