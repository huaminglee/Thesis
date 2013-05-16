using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;

namespace Crm
{
    public class AddressViewModel 
    {
        #region Properties

        public int AddressID { get; set; }

        public bool? IsActive { get; set; }

        public int? AddressType { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string Addition { get; set; }

        public string PostalCode { get; set; }

        public int? CityID { get; set; }

        public int? CountryID { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Description { get; set; }

        public string KeyPersonName { get; set; }

        public string KeyPersonPhone { get; set; }

        public int? ObjectType { get; set; }

        public DateTime? LastInvoiceDate { get; set; }

        public int? Electriciteit { get; set; }

        public string DetailDescription { get; set; }

        public string Location { get; set; }

        public string Information { get; set; }

        public string ExtraLetterText { get; set; }

        #endregion

        public string AddressTypeName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string ObjectTypeName { get; set; }
        public string ElectriciteitName { get; set; }

        #region Work Address Financials

        public int? PlannedManHours { get; set; }
        public decimal? TotalSurfaceM2 { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? InvoicedWorkAddress { get; set; }
        public decimal? ToBeInvoiced { get; set; }

        #endregion
    }
}
