using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class HBox : Ext.Net.Container, IBaseControl
    {
        public HBox()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (Width.IsEmpty)
                    Width = 930;

                if (string.IsNullOrEmpty(Layout))
                    Layout = "hbox";
            }
        }

        #endregion
    }
}
