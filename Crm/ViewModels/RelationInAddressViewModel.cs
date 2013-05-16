using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm
{
    public class RelationInAddressesViewModel
    {
        #region Properties

        public int RelationInAddressesID { get; set; }

        public int AddressID { get; set; }

        public int RelationID { get; set; }

        #endregion

        public string RelationName { get; set; }
        public string Address { get; set; }
    }
}
