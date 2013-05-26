using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class Checkbox : Ext.Net.Checkbox, IBaseControl
    {
        public Checkbox()
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
                if (string.IsNullOrEmpty(this.Name))
                    this.Name = this.ID;

                InputValue = bool.TrueString.ToLower();
            }
        }

        #endregion
    }
}
