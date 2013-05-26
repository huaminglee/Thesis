using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class ModuleRoles : Ext.Net.CheckboxGroup, IBaseControl        
    {
        public ModuleRoles()
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
                LabelSeparator = "";

                this.Items.AddRange(new List<Ext.Net.Checkbox>() { 
                    new Ext.Net.Checkbox() { ID = string.Format("{0}_View", this.ID) },
                    new Ext.Net.Checkbox() { ID = string.Format("{0}_Add", this.ID) },
                    new Ext.Net.Checkbox() { ID = string.Format("{0}_Update", this.ID) },
                    new Ext.Net.Checkbox() { ID = string.Format("{0}_Delete", this.ID) }
                });
            }
        }

        #endregion
    }
}
