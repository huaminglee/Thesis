using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shell
{
    public class FileViewModel
    {
        #region Properties

        public int FileID { get; set; }

        public string FileName { get; set; }

        public string Mimetype { get; set; }

        public int? Size { get; set; }

        public string Path { get; set; }

        public string Alias { get; set; }

        #endregion
    }
}
