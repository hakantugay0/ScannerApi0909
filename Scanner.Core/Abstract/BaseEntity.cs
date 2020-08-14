using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Abstract
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
