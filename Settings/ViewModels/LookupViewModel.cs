using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settings
{
    public class LookupViewModel
    {

        #region Properties

        public int LookupId { get; set; }

        public int LookupTypeId { get; set; }

        public List<LookupResourcesViewModel> LookupResources { get; set; }

        #endregion

    }


    public class LookupResourcesViewModel
    {

        #region Properties

        public string LanguageCode { get; set; }

        public string Text { get; set; }

        #endregion

    }
}
