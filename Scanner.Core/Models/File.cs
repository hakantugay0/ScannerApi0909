using Microsoft.VisualBasic;
using Scanner.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Scanner.Core.Models
{
    public class File : BaseEntity
    {
        public File()
        {
            Pages = new Collection<Page>();
        }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public int? FolderId { get; set; }
        public int UserId { get; set; }


        public virtual Folder Folder { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
    }
}
