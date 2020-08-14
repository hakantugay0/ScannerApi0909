using Scanner.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Models
{
    public class Log : BaseEntity
    {
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ResponseTime { get; set; }
    }
}
