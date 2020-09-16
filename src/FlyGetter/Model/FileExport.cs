using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyGetter.Model
{
    public class FileExport
    {
       public List<FileData> datas { get; set; }
    }

    public class FileData
    {
        public string name { get; set; }
        public List<string> items { get; set; }
    }
}
