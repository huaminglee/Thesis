using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.Services;

namespace Thesis.Web.UI.Controls
{
    public class MenuItem : Ext.Net.MenuItem, IBaseControl
    {
        public MenuItem()
        {
            SetDefaultValues = true;
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    LoadDefaultValues();
        //    base.OnInit(e);
        //}

        #region Properties

        public string Module { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            //if (SetDefaultValues)
            //{
                //if (this.Module != 0)
                  //  this.Visible = ModuleAuthorizeService.HasPermission(this.Module, ProcessTypes.View);
            //}
        }

        #endregion
    }
}
