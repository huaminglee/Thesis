using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.ComponentModel;
using System.Web.UI;

namespace Thesis.Web.UI.Controls
{    
    public class Column : Ext.Net.ColumnBase
    {
        public ColumnBaseType DataType { get; set; }
    }

    public enum ColumnBaseType
    { 
        String = 1,
        Int = 2,
        Float = 4,
        Date = 8,
        Bool = 16
    }
}
