using Microsoft.VisualBasic;
using Scanner.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Scanner.Core.Models
{
    public class Folder : BaseEntity
    {
        public Folder()
        {
            SubFolders = new Collection<Folder>();
            Files = new Collection<File>();
        }

        public string Name { get; set; }
        public int? SourceFolderId { get; set; }
        public int UserId { get; set; }


        public virtual User User { get; set; }
        public virtual Folder SourceFolder { get; set; }

        public virtual ICollection<Folder> SubFolders { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}
