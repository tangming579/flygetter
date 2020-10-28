using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyGetter.Model
{
    public class ResultModel<T> where T : class
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }
}
