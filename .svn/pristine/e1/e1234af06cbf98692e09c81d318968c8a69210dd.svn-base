using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class ResourceManager : Ext.Net.ResourceManager, IBaseControl
    {
        public ResourceManager()
        {
            SetDefaultValues = true;
            ShowWarningOnAjaxFailure = false;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool DisableRedirectLogin { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (!Ext.Net.ExtNet.IsAjaxRequest)
                {
                    base.RegisterIcon(Ext.Net.Icon.Information);
                    base.RegisterIcon(Ext.Net.Icon.Exclamation);
                }

                this.Locale = System.Globalization.CultureInfo.CurrentCulture.ToString();

                if (!DisableRedirectLogin)
                {
                    Listeners.AjaxRequestException.Handler += "if(response.status == 401 || (response.responseText && response.responseText.indexOf('<!-- Login Window -->') > -1)){window.location = '/Account/Login';}";
                }
            }
        }

        #endregion
    }
}
