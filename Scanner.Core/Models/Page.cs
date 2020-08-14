using Scanner.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Models
{
    public class Page : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; } //tr-TR veya en-US
        public int TotalWordCount { get; set; }
        public int FileId { get; set; }

        public virtual File File { get; set; }
    }
}
