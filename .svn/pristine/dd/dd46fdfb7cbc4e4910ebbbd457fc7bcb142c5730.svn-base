using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity
{
    public class ActivityViewModel
    {
        #region Properties

        public int ActivityID { get; set; }

        public int TypeID { get; set; }

        public int AddressID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsInvoiced { get; set; }

        public DateTime? RemainderDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int OwnerID { get; set; }

        public int ExecuterID { get; set; }

        public int? PlannedHours { get; set; }

        public int? ShiftID { get; set; }

        public decimal? Value { get; set; }

        public string InvoiceText { get; set; }

        public int? RelationID { get; set; }

        public int? InvoiceAddressID { get; set; }

        public int? DocumentID { get; set; }

        #endregion

        public string TypeName { get; set; }
        public string OwnerName { get; set; }
        public string ExecuterName { get; set; }
        public string Address { get; set; }
        public string ShiftNumber { get; set; }
        public string Relation { get; set; }
        public string InvoiceAddress { get; set; }

        private string number;
        public string Number {
            get
            {
                string zero = "000000";
                return zero.Substring(0, zero.Length - ActivityID.ToString().Length) + ActivityID.ToString();
            }
            set { number = value; } 
        }
    }
}
